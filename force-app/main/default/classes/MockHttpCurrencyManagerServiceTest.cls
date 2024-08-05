@isTest
global class MockHttpCurrencyManagerServiceTest implements HttpCalloutMock {
    global HTTPResponse respond(HTTPRequest req) {
        HttpResponse res = new HttpResponse();
        res.setHeader('Content-Type', 'application/json');
        res.setBody('{ "allOrNone" : false, "records" : [ { "attributes" : {"type" : "CurrencyType"}, "IsoCode" : "MXN", "DecimalPlaces" : 2, "ConversionRate" : 21, "IsActive" : "true" },{ "attributes" : {"type" : "CurrencyType"}, "IsoCode" : "PHP", "DecimalPlaces" : 2, "ConversionRate" : 21, "IsActive" : "true" }]}');
        res.setStatusCode(200);
        return res;
    }
}