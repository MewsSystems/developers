@IsTest
private class ExchangeRateControllerTest {
    @IsTest
    static void testGetExchangeRates() {
        // Create a mock HTTP response with sample XML data
        String sampleXmlData = '<kurzy banka="CNB" datum="09.05.2023" poradi="88">' +
                '<tabulka typ="XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU">' +
                '<radek kod="AUD" mena="dolar" mnozstvi="1" kurz="14,410" zeme="Austrálie"/>' +
                '<radek kod="BRL" mena="real" mnozstvi="1" kurz="4,258" zeme="Brazílie"/>' +
                '<radek kod="BGN" mena="lev" mnozstvi="1" kurz="11,948" zeme="Bulharsko"/>' +
                '<radek kod="CNY" mena="žen-min-pi" mnozstvi="1" kurz="3,079" zeme="Čína"/>' +
                '<radek kod="DKK" mena="koruna" mnozstvi="1" kurz="3,138" zeme="Dánsko"/>' +
                '</tabulka>' +
                '</kurzy>';
        
        Test.setMock(HttpCalloutMock.class, new ExchangeMock());
        
        // Start the test
        Test.startTest();
      
        
        // Call the method under test
        List<ExchangeRateController.ExchangeRate> exchangeRates = ExchangeRateController.getExchangeRates();
        
        // Verify the results
        System.assertEquals(5, exchangeRates.size());
        
        // Verify the first exchange rate
        ExchangeRateController.ExchangeRate firstRate = exchangeRates[0];
        System.assertEquals('Austrálie', firstRate.country);
        System.assertEquals('dolar', firstRate.currencyName);
        System.assertEquals(1, firstRate.quantity);
        System.assertEquals('AUD', firstRate.code);
        System.assertEquals(14.410, firstRate.exchangeRate);
        
        // Verify the second exchange rate
        ExchangeRateController.ExchangeRate secondRate = exchangeRates[1];
        System.assertEquals('Brazílie', secondRate.country);
        System.assertEquals('real', secondRate.currencyName);
        System.assertEquals(1, secondRate.quantity);
        System.assertEquals('BRL', secondRate.code);
        System.assertEquals(4.258, secondRate.exchangeRate);
        
        Test.stopTest();
    }
}