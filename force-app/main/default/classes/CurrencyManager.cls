public with sharing class CurrencyManager {
    private static String CURRENCY_URL = 'https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=';
    
    public static String getTodayCurrencyRates(){
        CURRENCY_URL += getCurrentDateFormatted();
        return ExchangeRateFileReader.getFileFromSource(CURRENCY_URL);
    }

    public static String getCurrentDateFormatted() {
        Integer day = Date.today().day();
        Integer month = Date.today().month();
        Integer year = Date.today().year();
        
        String dayString = day < 10 ? '0' + String.valueOf(day) : String.valueOf(day);
        String monthString = month < 10 ? '0' + String.valueOf(month) : String.valueOf(month);
        
        String formattedDate = dayString + '.' + monthString + '.' + String.valueOf(year);
        
        return formattedDate;
    }

    
    public static String upsertCurrencies(){
        
        String fileContent = getTodayCurrencyRates();
        if(fileContent == null){
            
           return 'Error 304';
        }
        
        Http http = new Http();
        HttpRequest request = new HttpRequest();
        request.setEndpoint(System.URL.getOrgDomainUrl().toExternalForm()  + '/services/data/v61.0/composite/sobjects/');
        request.setTimeout(60000);
        request.setMethod('POST');
        request.setBody(fileContent);
        request.setHeader('Authorization', 'OAuth ' + UserInfo.getSessionId());
        request.setHeader('Content-Type', 'application/json');
        HttpResponse res = http.send(request);

       return res.getBody().toString();
    }
    @InvocableMethod(label='Upsert Currencies' description='Upsert currencies from CNB')
    public static void upsertCurrenciesFlowCallout(){
        upsertCurrencies();   
    }

}