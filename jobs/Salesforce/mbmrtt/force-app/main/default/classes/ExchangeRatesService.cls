/**
 * @description Service class to hold logic for managing the Exchange Rates logic.
 * The class uses REST http callouts to get the Exchange Rates from and to Czech Koruna via Named Credential endpoint
 * Swagger file = https://api.cnb.cz/cnbapi/swagger-ui.html
 *
 * Execute Anonymous: ExchangeRatesService.getRates(System.now());
 */
public with sharing class ExchangeRatesService {
    @TestVisible
    private static final String CNB_ENDPOINT = 'callout:Czech_National_Bank_Exchange_Rates';
    @TestVisible
    private static final String ASK_HELP = 'Please contact your System Admin for help.';
    @TestVisible
    private static final String CNB_DEFAULT_CURRENCY_NAME = 'Czech - Koruna';
    @TestVisible
    private static final String CNB_DEFAULT_CURRENCY_CODE = 'CZK';

    /**
     * @description Method to allow the package user to test the installation of the package
     * directly from Anonymous Execution without waiting for the scheduled job to run
     */
    public static void testGetRates() {
        ExchangeRatesService.getRates(System.now());
    }

    /**
     * @description Method to get the Exchange Rates from a given Named Credential endpoint
     * and save the result back into SF as sObject records. This main scope will also determins the error message.
     *
     * @param requestedDate DateTime for which the exchange rates should be fetched
     */
    @Future(callout=true)
    public static void getRates(Datetime requestedDate) {
        // create a new HttpRequest and validate the provided param
        HttpRequest req = createHttpRequest(requestedDate);
        if (req == null) {
            handleError('Could not create a valid HttpRequest. ' + ASK_HELP);
            return;
        }

        // make callout and process the response
        HTTPResponse res = new Http().send(req);

        if (res.getStatusCode() != 200) {
            // Bad Request | Not Found | Internal server error
            handleError('Could not update this record due to an external ' + res.getStatusCode() + ' error code. ' + ASK_HELP);
            return;
        }

        String rawResponse = res.getBody();
        String securedResponse = rawResponse.replace('currency', 'currencyName'); // replacing reserved identifiers
        Rates fetchedRates = (Rates) JSON.deserialize(securedResponse, Rates.class);

        // the endpoint will return an empty Rates object if there is no data for the given date
        // over the top safty check in case the records start date will change in the future following a cleanup
        if (fetchedRates.rates.size() == 0 || fetchedRates.rates[0].validFor != (requestedDate).format('yyyy-MM-dd')) {
            handleError('Could not find data for the requested date.');
        }

        Boolean updatedRecords = updateExchangeRates(fetchedRates.rates);

        if (updatedRecords == false) {
            handleError('Could not update this record due to internal processing error. ' + ASK_HELP);
        }
    }

    /**
     * @description Method to update SF with the fetched exchange rates
     * using the object External Id
     *
     * @return Boolean indicator if all possible rates are updated, returns false if there was even 1 failure
     *
     * @param rates List<ExchangeRatesService.CurrencyRate> to be processed
     */
    @TestVisible
    private static Boolean updateExchangeRates(List<CurrencyRate> rates) {
        List<Currency_Rate__c> ratesToUpsert = new List<Currency_Rate__c>();
        for (CurrencyRate rate : rates) {
            String country = fixCapitalization(rate.country);
            String currencyName = fixCapitalization(rate.currencyName);
            ratesToUpsert.add(
                new Currency_Rate__c(
                    Name = CNB_DEFAULT_CURRENCY_CODE + '_' + rate.currencyNameCode,
                    Base_Amount__c = rate.rate,
                    Base_Code__c = CNB_DEFAULT_CURRENCY_CODE,
                    Base_Currency__c = CNB_DEFAULT_CURRENCY_NAME,
                    Error_Message__c = '',
                    Target_Amount__c = rate.amount,
                    Target_Code__c = rate.currencyNameCode,
                    Target_Currency__c = country + ' - ' + currencyName
                )
            );

            ratesToUpsert.add(
                new Currency_Rate__c(
                    Name = rate.currencyNameCode + '_' + CNB_DEFAULT_CURRENCY_CODE,
                    Base_Amount__c = rate.amount,
                    Base_Code__c = rate.currencyNameCode,
                    Base_Currency__c = country + ' - ' + currencyName,
                    Error_Message__c = '',
                    Target_Amount__c = rate.amount / rate.rate,
                    Target_Code__c = CNB_DEFAULT_CURRENCY_CODE,
                    Target_Currency__c = CNB_DEFAULT_CURRENCY_NAME
                )
            );
        }

        // update as many records as possible, and if there was even 1 failure return false
        List<Database.UpsertResult> results = Database.upsert(ratesToUpsert, Currency_Rate__c.Name, false);
        for (Database.upsertResult result : results) {
            if (result.isSuccess() == false) {
                return false;
            }
        }
        return true;
    }

    /**
     * @description Util Method to capitalize given sentences
     *
     * @return String sentece where every word is capitalized
     *
     * @param sentence String where every word should start with an uppercase
     */
    @TestVisible
    private static String fixCapitalization(String sentence) {
        // check if not already uppercase (sign for abbreviation)
        if (sentence.equals(sentence.toUpperCase())) {
            return sentence;
        }

        List<String> words = sentence.toLowerCase().split(' ');
        List<String> capitalized = new List<String>();
        for (String word : words) {
            capitalized.add(word.capitalize());
        }
        return String.join(capitalized, ' ');
    }

    /**
     * @description Method to handle the creation of a new HttpRequest
     *
     * @return HttpRequest with endpoint, and GET method
     *
     * @param requestedDate DateTime for which the exchange rates should be fetched
     */
    @TestVisible
    private static HttpRequest createHttpRequest(Datetime requestedDate) {
        // set the requested date for the callout
        String endPoint = getEndpoint(requestedDate);

        if (String.isBlank(endPoint)) {
            return null;
        }

        HttpRequest req = new HttpRequest();
        req.setEndpoint(endPoint);
        req.setMethod('GET');

        return req;
    }

    /**
     * @description Method to validate the requested date for the API call.
     * While the endpoint will provide a valid response (https://api.cnb.cz/cnbapi/swagger-ui.html) using today's date
     * We want don't want to provide misleading information
     *
     * @return String endpoint containing a valid requested date
     *
     * @param requestedDate DateTime for which the exchange rates should be fetched
     */
    @TestVisible
    private static String getEndpoint(DateTime requestedDate) {
        // validate param to prevent an invalid API call
        if (requestedDate == null || requestedDate > System.now() || requestedDate.year() < 1991) {
            return null;
        }
        // return processed date in the expected format
        return CNB_ENDPOINT + '/daily?date=' + (requestedDate).format('yyyy-MM-dd') + '&lang=EN';
    }

    /**
     * @description Method to handle errors for the request
     * suitable scope for possibility throwing an exception, inserting a new logger record, notifying admin via email, use an event to update UI, etc.
     * in the example below, we are updating the Koruna based records that didn't get updated in the update window with the given error
     *
     * @param errorMsg String - custom text or direct from the endpoint
     */
    @TestVisible
    private static void handleError(String errorMsg) {
        Datetime updateWindow = System.Now().addHours(-3);
        List<Currency_Rate__c> rates = [
            SELECT Id
            FROM Currency_Rate__c
            WHERE Base_Currency__c = 'Czech Koruna' AND LastModifiedDate < :updateWindow
            WITH SYSTEM_MODE
        ];

        for (Currency_Rate__c rate : rates) {
            rate.Error_Message__c = errorMsg;
        }

        // for records that will fail in the DML, we use the Status__c formula as a fallback
        Database.update(rates, false);
    }

    /**
     * @description Decorator class to represent an individual country rate from Czech_National_Bank_Exchange_Rates
     */
    @TestVisible
    private class CurrencyRate {
        public String validFor { get; set; }
        public Integer order { get; set; }
        public String country { get; set; }
        public String currencyName { get; set; } // currency name, for ex: dollar - not unique
        public Integer amount { get; set; }
        public String currencyNameCode { get; set; } // ISO Code
        public Decimal rate { get; set; }
    }

    /**
     * @description Decorator class to process the response from Czech_National_Bank_Exchange_Rates
     */
    @TestVisible
    private class Rates {
        public List<CurrencyRate> rates { get; set; }
    }
}