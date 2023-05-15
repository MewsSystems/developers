@isTest
public class ExchangeMock implements HttpCalloutMock {
    public HTTPResponse respond(HTTPRequest req) {
        HttpResponse res = new HttpResponse();
        res.setHeader('Content-Type', 'application/xml');
         res.setBody('<kurzy banka="CNB" datum="09.05.2023" poradi="88">' +
                '<tabulka typ="XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU">' +
                '<radek kod="AUD" mena="dolar" mnozstvi="1" kurz="14,410" zeme="Austrálie"/>' +
                '<radek kod="BRL" mena="real" mnozstvi="1" kurz="4,258" zeme="Brazílie"/>' +
                '<radek kod="BGN" mena="lev" mnozstvi="1" kurz="11,948" zeme="Bulharsko"/>' +
                '<radek kod="CNY" mena="žen-min-pi" mnozstvi="1" kurz="3,079" zeme="Čína"/>' +
                '<radek kod="DKK" mena="koruna" mnozstvi="1" kurz="3,138" zeme="Dánsko"/>' +
                '</tabulka>' +
                '</kurzy>');      
        res.setStatusCode(200);
        return res;
    }
}