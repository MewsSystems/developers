@isTest
private class ExchangeRateFileReaderTest {

@isTest
    static void testFormatToJson() {
        String testFileContent = '01 Aug 2024 #9999 \n' +
                                    'Country|Currency|Amount|Code|Rate \n' +
                                    'Australia|dollar|1|AUD|15.428\n' +
                                    'Hongkong|dollar|1|HKD|3.019\n';
        Test.startTest();
        String actualJson = ExchangeRateFileReader.formatToJson(testFileContent);
        Test.stopTest();
        System.debug('actualJson: ' + actualJson);
        System.assertNotEquals(true, actualJson.contains('01 Aug 2024 #9999'));
        System.assertEquals(true, actualJson.contains('CurrencyType'));
    }
}