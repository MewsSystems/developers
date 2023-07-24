@IsTest
public with sharing class ExchangeRatesServiceTest {
    /*****************************
     * Testing Scope without DML *
     *****************************/

    @IsTest
    public static void test_getEndpoint() {
        DateTime requestedDate = null;
        String endpoint = ExchangeRatesService.getEndpoint(requestedDate);
        Assert.isNull(endpoint, 'Expected a null value to return for a null input');

        requestedDate = System.now().addDays(10);
        endpoint = ExchangeRatesService.getEndpoint(requestedDate);
        Assert.isNull(endpoint, 'Expected a null value to return for a future date');

        requestedDate = Datetime.newInstance(1990, 12, 29, 0, 0, 0);
        endpoint = ExchangeRatesService.getEndpoint(requestedDate);
        Assert.isNull(endpoint, 'Expected a null value to return for a date before 1991');

        requestedDate = Datetime.newInstance(1991, 1, 1, 0, 0, 0);
        endpoint = ExchangeRatesService.getEndpoint(requestedDate);
        Assert.isNotNull(endpoint, 'Did not expect a null value for a date within the valid range');

        requestedDate = System.now();
        endpoint = ExchangeRatesService.getEndpoint(requestedDate);
        Assert.isNotNull(endpoint, 'Did not expect a null value for a date within the valid range');

        Assert.isTrue(
            endpoint.contains(ExchangeRatesService.CNB_ENDPOINT + '/daily?date='),
            'Seems like the endpoint changed without updating the test'
        );
    }

    @IsTest
    public static void test_getRates_e2e_noError_InvalidDate() {
        Boolean testPassed = true;

        Test.startTest();
        try {
            Test.setMock(HttpCalloutMock.class, new MockHttpResponseGenerator(200, false));
            ExchangeRatesService.getRates(Datetime.newInstance(1990, 12, 29, 0, 0, 0));
        } catch (Exception ex) {
            testPassed = false;
        }
        Test.stopTest();

        Assert.isTrue(testPassed, 'Expected the logic to run without throwing an error.');
    }

    @IsTest
    public static void test_getRates_e2e_noError_ValidDateNoData() {
        Boolean testPassed = true;

        Test.startTest();
        try {
            Test.setMock(HttpCalloutMock.class, new MockHttpResponseGenerator(200, false));
            ExchangeRatesService.getRates(System.now());
        } catch (Exception ex) {
            testPassed = false;
        }
        Test.stopTest();

        Assert.isTrue(testPassed, 'Expected the logic to run without throwing an error.');
    }

    @IsTest
    public static void test_getRates_e2e_noError_200() {
        Boolean testPassed = true;

        Test.startTest();
        try {
            Test.setMock(HttpCalloutMock.class, new MockHttpResponseGenerator(200, true));
            ExchangeRatesService.getRates(System.now());
        } catch (Exception ex) {
            testPassed = false;
        }
        Test.stopTest();

        Assert.isTrue(testPassed, 'Expected the logic to run without throwing an error.');
    }

    @IsTest
    public static void test_getRates_e2e_noError_404() {
        Boolean testPassed = true;

        Test.startTest();
        try {
            Test.setMock(HttpCalloutMock.class, new MockHttpResponseGenerator(404, true));
            ExchangeRatesService.getRates(System.now());
        } catch (Exception ex) {
            testPassed = false;
        }
        Test.stopTest();

        Assert.isTrue(testPassed, 'Expected the logic to run without throwing an error.');
    }

    /**
     * @description Nested class to mock an HTTP response for ExchangeRatesService
     */
    public class MockHttpResponseGenerator implements HttpCalloutMock {
        private Integer code { get; set; }
        private Boolean returnData { get; set; }

        public MockHttpResponseGenerator(Integer code, Boolean returnData) {
            this.code = code;
            this.returnData = returnData;
        }

        /**
         * @description Method to return a mock HTTP Response for the endpoint Czech_National_Bank_Exchange_Rates
         *
         * @return HttpResponse mocked based on the expected API response
         *
         * @param req HTTPRequest to be mocked
         */
        public HttpResponse respond(HTTPRequest req) {
            // Create a fake response
            HttpResponse res = new HttpResponse();
            res.setHeader('Content-Type', 'application/json');
            res.setStatusCode(code);

            if (code != 200) {
                String error = '{"description": "string","endPoint": "string","errorCode": "INTERNAL_SERVER_ERROR","happenedAt": "2023-07-14T11:00:00","messageId": "string"}';
                res.setBody(error);
                return res;
            }

            if (code == 200 && returnData == false) {
                res.setBody('{"rates":[]}');
                return res;
            }

            ExchangeRatesService.CurrencyRate rate1 = new ExchangeRatesService.CurrencyRate();
            rate1.validFor = '';
            rate1.order = 1;
            rate1.country = 'Australia';
            rate1.currencyName = 'dollar';
            rate1.amount = 1;
            rate1.currencyNameCode = 'AUD';
            rate1.rate = 24.54;

            ExchangeRatesService.CurrencyRate rate2 = new ExchangeRatesService.CurrencyRate();
            rate2.validFor = '';
            rate2.order = 1;
            rate2.country = 'Canada';
            rate2.currencyName = 'dollar';
            rate2.amount = 1;
            rate2.currencyNameCode = 'CAD';
            rate2.rate = 26.145;

            ExchangeRatesService.Rates mockedRates = new ExchangeRatesService.Rates();
            mockedRates.rates = new List<ExchangeRatesService.CurrencyRate>{ rate1, rate2 };

            String mockedSecuredResponse = JSON.serialize(mockedRates);
            String mockedRawResponse = mockedSecuredResponse.replace('currencyName', 'currency');

            res.setBody(mockedRawResponse);
            return res;
        }
    }

    /**************************
     * Testing Scope with DML *
     **************************/

    @IsTest
    static void methodName() {
        List<Currency_Rate__c> rates = getDummyRates();
        insert rates;

        // mock data includes AUD and CAD, while dummy data includes AUD, CAD, HUF, INR and IDR
        List<Currency_Rate__c> commonRates = [
            SELECT Id, Name, Base_Amount__c, Base_Code__c, Base_Currency__c, Error_Message__c, Target_Amount__c, Target_Code__c, Target_Currency__c
            FROM Currency_Rate__c
            WHERE Target_Code__c = 'CAD' OR Target_Code__c = 'AUD'
        ];

        for (Currency_Rate__c rate : commonRates) {
            if (rate.Target_Code__c == 'CAD') {
                Assert.areEqual(16.145, rate.Base_Amount__c);
                Assert.areEqual(1, rate.Target_Amount__c);
            } else {
                Assert.areEqual(14.54, rate.Base_Amount__c);
                Assert.areEqual(1, rate.Target_Amount__c);
            }
        }

        Boolean testPassed = true;

        Test.startTest();
        try {
            Test.setMock(HttpCalloutMock.class, new MockHttpResponseGenerator(200, true));
            ExchangeRatesService.testGetRates();
        } catch (Exception ex) {
            testPassed = false;
        }
        Test.stopTest();

        // mock data includes AUD and CAD, while dummy data includes AUD, CAD, HUF, INR and IDR
        Assert.isTrue(testPassed, 'Expected the logic to run without throwing an error.');

        commonRates = [
            SELECT Id, Name, Base_Amount__c, Base_Code__c, Base_Currency__c, Error_Message__c, Target_Amount__c, Target_Code__c, Target_Currency__c
            FROM Currency_Rate__c
            WHERE Target_Code__c = 'CAD' OR Target_Code__c = 'AUD'
        ];

        for (Currency_Rate__c rate : commonRates) {
            if (rate.Target_Code__c == 'CAD') {
                Assert.areEqual(26.145, rate.Base_Amount__c);
                Assert.areEqual(1, rate.Target_Amount__c);
            } else {
                Assert.areEqual(24.54, rate.Base_Amount__c);
                Assert.areEqual(1, rate.Target_Amount__c);
            }
        }
    }

    private static List<Currency_Rate__c> getDummyRates() {
        List<Currency_Rate__c> rates = new List<Currency_Rate__c>();

        rates.add(
            new Currency_Rate__c(
                Name = 'CZK_CAD',
                Base_Amount__c = 16.145,
                Base_Code__c = 'CZK',
                Base_Currency__c = 'Czech Koruna',
                Error_Message__c = '',
                Target_Amount__c = 1,
                Target_Code__c = 'CAD',
                Target_Currency__c = 'Canada - Dollar'
            )
        );

        rates.add(
            new Currency_Rate__c(
                Name = 'CZK_AUD',
                Base_Amount__c = 14.54,
                Base_Code__c = 'CZK',
                Base_Currency__c = 'Czech Koruna',
                Error_Message__c = '',
                Target_Amount__c = 1,
                Target_Code__c = 'AUD',
                Target_Currency__c = 'Australia - Dollar'
            )
        );

        rates.add(
            new Currency_Rate__c(
                Name = 'CZK_HUF',
                Base_Amount__c = 6.352,
                Base_Code__c = 'CZK',
                Base_Currency__c = 'Czech Koruna',
                Error_Message__c = '',
                Target_Amount__c = 100,
                Target_Code__c = 'HUF',
                Target_Currency__c = 'Hungary - Rupiah'
            )
        );

        rates.add(
            new Currency_Rate__c(
                Name = 'CZK_INR',
                Base_Amount__c = 25.791,
                Base_Code__c = 'CZK',
                Base_Currency__c = 'Czech Koruna',
                Error_Message__c = '',
                Target_Amount__c = 100,
                Target_Code__c = 'INR',
                Target_Currency__c = 'India - Rupee'
            )
        );

        rates.add(
            new Currency_Rate__c(
                Name = 'CZK_IDR',
                Base_Amount__c = 1.417,
                Base_Code__c = 'CZK',
                Base_Currency__c = 'Czech Koruna',
                Error_Message__c = '',
                Target_Amount__c = 1000,
                Target_Code__c = 'IDR',
                Target_Currency__c = 'Indonesia - Rupiah'
            )
        );

        return rates;
    }
}