@isTest
private class CurrencyManagerServiceTest  {

    @isTest
    static void testGetTodayCurrencyRates() {
        Test.startTest();
        String result = CurrencyManagerService.getTodayCurrencyRates();
        Test.stopTest();
        System.assertNotEquals(null, result, 'Result should not be null');
    }


@isTest
    static void testUpsertCurrencies() {
        Test.setMock(HttpCalloutMock.class, new MockHttpCurrencyManagerServiceTest());
        String res = CurrencyManagerService.upsertCurrencies();
        
        System.assertNotEquals(null, res, 'Result should not be null');
        
    }
    

    
}