@isTest
private class ExchangeRatesPanelControllerTest {

@isTest
    static void testGetActiveCurrencies() {
        List<CurrencyType> currencyList = new List<CurrencyType>();
        Test.startTest();
        
            currencyList = ExchangeRatesPanelController.getActiveCurrencies();
            
        Test.stopTest();
        
        System.assertNotEquals(null, currencyList.size());
    }
}