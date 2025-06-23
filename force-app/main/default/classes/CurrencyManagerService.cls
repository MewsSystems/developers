@RestResource(urlMapping='/CurrencyManager/*')
global with sharing class CurrencyManagerService {
    
    @HttpGet
    global static String getTodayCurrencyRates(){ 
        return CurrencyManager.getTodayCurrencyRates();
    }

    @HttpPost
    global static String upsertCurrencies() {
        return CurrencyManager.upsertCurrencies();
    }

    
}