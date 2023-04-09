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
    }
}