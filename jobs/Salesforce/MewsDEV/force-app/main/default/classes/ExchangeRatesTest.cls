@isTest
/**
 * Test class for ExchangeRates Class, using ExchangeRatesTestMock class
 */
public with sharing class ExchangeRatesTest {
    @isTest
    private static void getExchangeRatesTest(){
        Test.setMock(HttpCalloutMock.class, new ExchangeRatesTestMock());
        Test.startTest();
        ExchangeRates.getExchangeRates();
        Test.stopTest();
        List<Exchange_Rate__c> result = [SELECT ID from Exchange_Rate__c];
        System.assertEquals(6, result.size());
        Exchange_Rate__c auDolar = [SELECT ID, Country__c, Amount__c, Rate__c from Exchange_Rate__C where ISO_Code__c = 'AUD' limit 1];
        System.assertEquals(1, auDolar.Amount__c);
        System.assertEquals(14.351, auDolar.Rate__c);
        System.assertEquals('Austrálie', auDolar.Country__c);
    }
}