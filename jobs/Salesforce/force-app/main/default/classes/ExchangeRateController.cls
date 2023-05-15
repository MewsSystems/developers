public with sharing class ExchangeRateController {

    public class ExchangeRate {
        @AuraEnabled public String country;
        @AuraEnabled public String currencyName;
        @AuraEnabled public Integer quantity;
        @AuraEnabled public String code;
        @AuraEnabled public Decimal exchangeRate;
          @AuraEnabled
        public String exchangeRateDate { get; set; }

        public ExchangeRate(String country, String currencyName, Integer quantity, String code, Decimal exchangeRate, String exchangeRateDate) {
            this.country = country;
            this.currencyName = currencyName;
            this.quantity = quantity;
            this.code = code;
            this.exchangeRate = exchangeRate;
            this.exchangeRateDate = exchangeRateDate;
        }
    }

    @AuraEnabled(cacheable=true)
    public static List<ExchangeRate> getExchangeRates() {
        List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

        try {
            HttpRequest request = new HttpRequest();
            request.setEndpoint('callout:cnb');
            request.setMethod('GET');

            Http http = new Http();
            HttpResponse response = http.send(request);

            if (response.getStatusCode() == 200) {
                String xmlData = response.getBody();
                Dom.Document document = new Dom.Document();
                document.load(xmlData);

                Dom.XmlNode rootNode = document.getRootElement();
                Dom.XmlNode tabulkaNode = getChildElement(rootNode, 'tabulka');
                
                System.debug( ' date ' + tabulkaNode);
                String exchangeRateDate = rootNode.getAttributeValue('datum', null);

                List<Dom.XmlNode> exchangeRateNodes = getChildElements(tabulkaNode, 'radek');

                for (Dom.XmlNode exchangeRateNode : exchangeRateNodes) {
                    String country = exchangeRateNode.getAttributeValue('zeme', null);
                    String currencyName = exchangeRateNode.getAttributeValue('mena', null);
                    Integer quantity = Integer.valueOf(exchangeRateNode.getAttributeValue('mnozstvi', null));
                    String code = exchangeRateNode.getAttributeValue('kod', null);
                    Decimal exchangeRate = Decimal.valueOf(exchangeRateNode.getAttributeValue('kurz', null).replace(',', '.'));

                    ExchangeRate rate = new ExchangeRate(country, currencyName, quantity, code, exchangeRate, exchangeRateDate);
                    exchangeRates.add(rate);
                    
					System.debug(' fetching exchange rates: ' +exchangeRates);

                }
            }
        } catch (Exception e) {
            System.debug('Error fetching exchange rates: ' + e.getMessage());
        }

        return exchangeRates;
    }

    private static Dom.XmlNode getChildElement(Dom.XmlNode parentNode, String tagName) {
        if (parentNode != null && tagName != null) {
            List<Dom.XmlNode> childElements = parentNode.getChildElements();
            for (Dom.XmlNode childElement : childElements) {
                if (childElement.getName().equals(tagName)) {
                    return childElement;
                }
            }
        }
        return null;
    }

    private static List<Dom.XmlNode> getChildElements(Dom.XmlNode parentNode, String tagName) {
        List<Dom.XmlNode> childElements = new List<Dom.XmlNode>();
        if (parentNode != null && tagName != null) {
            List<Dom.XmlNode> children = parentNode.getChildElements();
            for (Dom.XmlNode child : children) {
                if (child.getName().equals(tagName)) {
                    childElements.add(child);
                }
            }
        }
        return childElements;
    }
}