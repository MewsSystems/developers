public with sharing class ExchangeRates implements Schedulable, Database.AllowsCallouts {

    public static void execute(SchedulableContext sc){
        getExchangeRates();
    }

    /**
     * call CNB API and store values in custom objects
     */

     @future(callout=true)
    public static void getExchangeRates(){
        Http http = new Http();
        HttpRequest request = new HttpRequest();
        request.setMethod('GET');
        if(Test.isRunningTest()){
            request.setEndpoint('https://www.cnb.cz');
        }
        else{
            request.setEndpoint('callout:CNB_Exchange_Rates');
        }
        try {
            HttpResponse response = http.send(request);
            List<Exchange_Rate__c> ratesToCreate = parseTXT(response.getBody());

            SObjectAccessDecision securityDecision = Security.stripInaccessible(AccessType.CREATABLE, ratesToCreate);
            insert securityDecision.getRecords();
        } catch (Exception ex) {
            throw new CalloutException('Get Exchange Rates failed: ' + ex);
        }        
    }

    /**
     * parse txt and return list of Exchange_Rate custom objects
     */
    private static List<Exchange_Rate__c> parseTXT(String body){

        List<Exchange_Rate__c> ratesToInsert = new List<Exchange_Rate__c>();
        Date rateDate;
        List<String> lines = body.split('\n');
        for(String line: lines){
            System.debug(line);
            //first line = date
            if(line.contains('#')){
                List<String> firstLine = line.split('#');
                rateDate = Date.parse(firstLine.get(0).trim());                
            }
            else {   
                List<String> elements = line.split('\\|');
                if (elements.get(2).isNumeric()){                       //to bypass second line (header)
                    Exchange_Rate__c exRate = new Exchange_Rate__c();
                    exRate.Date_of_Rate__c = rateDate;
                    exRate.Country__c = elements.get(0);
                    exRate.Currency__c = elements.get(1);
                    exRate.Amount__c = Integer.valueOf(elements.get(2));
                    exRate.ISO_code__c = elements.get(3);
                    exRate.rate__c = Decimal.valueOf(elements.get(4).replace(',', '.'));
                    ratesToInsert.add(exRate);
                }
            }
        }
        return ratesToInsert;
    }
}