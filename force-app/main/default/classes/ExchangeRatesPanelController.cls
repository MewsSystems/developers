public with sharing class ExchangeRatesPanelController {
    @AuraEnabled(Cacheable=true)
    public static List<CurrencyType> getActiveCurrencies(){
        return [SELECT Id , IsoCode, ConversionRate, LastModifiedDate  FROM CurrencyType WHERE IsActive = true];
    }    
}