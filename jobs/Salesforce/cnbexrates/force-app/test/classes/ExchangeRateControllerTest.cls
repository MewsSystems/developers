@isTest 
public class ExchangeRateControllerTest {
    
    @isTest 
    public static void getExchangeRates() {
        ExchangeRateProvider.setMock(
            Test.createStub(ExchangeRateProvider.class, new TestMocks.MockProvider())
        );

        Test.startTest();
        List<ExchangeRate> rates = ExchangeRateController.getExchangeRates();
        Test.stopTest();

        System.assertEquals(3, rates.size(), 'List of rates does not have expected size of 3');
        System.assertEquals('ABC', rates[0].currencyCode, 'First rate is not for expected currency ABC');
    }

    @isTest 
    public static void getExchangeRates_Exception() {
        Exception error;
        ExchangeRateProvider.setMock(
            Test.createStub(ExchangeRateProvider.class, new TestMocks.MockProviderError())
        );
         
        Test.startTest();
        try {
            List<ExchangeRate> rates = ExchangeRateController.getExchangeRates();
        }
        catch(Exception ex) {
            error = ex;
        }
        Test.stopTest();

        System.assertEquals('System.AuraHandledException', error.getTypeName(), 'Exception type is not AuraHandledException');
    }
    
}