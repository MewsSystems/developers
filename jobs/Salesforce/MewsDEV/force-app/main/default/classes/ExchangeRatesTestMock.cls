@isTest
/**
 * Mock Class for ExchangeRatesTest class
 */
public with sharing class ExchangeRatesTestMock implements HttpCalloutMock{
    public HttpResponse respond (HttpRequest req){
        System.assertEquals('GET', req.getMethod());
        System.assertEquals('https://www.cnb.cz', req.getEndpoint());

        HttpResponse response = new HttpResponse();
        response.setStatusCode(200);
        String body = '06.04.2023 #69\n'+ 
        'země|měna|množství|kód|kurz\n'+
        'Austrálie|dolar|1|AUD|14,351\n'+
        'Brazílie|real|1|BRL|4,251\n'+
        'Bulharsko|lev|1|BGN|11,969\n'+
        'Čína|žen-min-pi|1|CNY|3,122\n'+
        'Filipíny|peso|100|PHP|39,309\n'+
        'Indonesie|rupie|1000|IDR|1,438\n';
        response.setBody(body);
        return response;
    }
}