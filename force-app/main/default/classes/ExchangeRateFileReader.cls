public with sharing class ExchangeRateFileReader  {
    
    public static String getFileFromSource(String fileUrl) {
        
        try {
            Http http = new Http();
            HttpRequest request = new HttpRequest();
            request.setEndpoint(fileUrl);
            request.setMethod('GET');
            HttpResponse response = http.send(request);
            String fileContent = null;
            if (response.getStatusCode() == 200) {
                fileContent = response.getBody();
                fileContent = formatToJson(fileContent);
                
            }
        return fileContent;
            
        } catch (Exception e) {
            System.debug('Exeption catched, Error downloading the file: ' + e.getMessage());
            return 'Error 500';
        }

    }

    public static String formatToJson(String fileContent) {
            if (fileContent == null) {
                return null;
            }
            fileContent = stripFirstLine(fileContent);
            
            List<String> contentInLines = fileContent.split('\n');
            
            if (contentInLines.size() < 2) {
                return null;
            }

            
            CurrencyExchange__mdt CurrencyExchange = [SELECT Id, IsoCode_Index__c,ConversionRate_Index__c,DecimalPlaces__c, AllOrNone__c 
                                                        FROM CurrencyExchange__mdt 
                                                        WHERE isActive__c = TRUE 
                                                        LIMIT 1];
     
            System.JSONGenerator ct = System.JSON.createGenerator(true);
            ct.writeStartObject();
            ct.writeBooleanField('allOrNone', CurrencyExchange.AllOrNone__c);
            ct.writeFieldName('records');
            ct.writeStartArray();
            
            for (Integer i = 1; i < contentInLines.size(); i++) {
               
                List<String> values = contentInLines[i].split('\\|');
                if(values.isEmpty()){
                    continue;
                }
       
                ct.writeStartObject();

                ct.writeFieldName('attributes');
                ct.writeStartObject();
                ct.writeStringField('type', 'CurrencyType');
                ct.writeEndObject();


                ct.writeStringField('IsoCode', String.valueOf(values[Integer.valueOf(CurrencyExchange.IsoCode_Index__c)]));
                ct.writeNumberField('ConversionRate', (1 / Decimal.valueOf(values[Integer.valueOf(CurrencyExchange.ConversionRate_Index__c)])));
                ct.writeBooleanField('IsActive', true);
                ct.writeNumberField('DecimalPlaces', CurrencyExchange.DecimalPlaces__c);
                ct.writeEndObject();
                
            }
            ct.writeEndArray();
            ct.writeEndObject();
            
            String jsonOutput = ct.getAsString();
            return jsonOutput;
        }
    
    

    private static String stripFirstLine(String content) {
        if (content != null) {
            String newContent = content;
            newContent =  newContent.substring(newContent.indexOf('Country'));
            return newContent;
        } else {
            return null;
        }
    }
    

}