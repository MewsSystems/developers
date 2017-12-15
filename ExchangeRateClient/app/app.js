import { endpoint, interval } from './config';

var vueInstance1;
window.onload = function () {
  vueInstance1 = new Vue({
   el: "#exchange-rate-client",
    data: {
      exchangeRateServer: "http://localhost:3000/",
      configAPI: "configuration",
      ratesAPI: "rates",
      show: false,
      currencyPairs: [],
      currenciesRates: [],
      currencyRatesTrends: [],
      currencyPairsFiltered: []
    }, 
    methods: {
     init: function(){
       this.getCurrencyPairs();
       this.getRates();

       setInterval(function() { 
          this.refreshSelectedRates();
       }.bind(this) ,3000);
     }, 
     getCurrencyPairs: function() {
        axios.get(this.exchangeRateServer + this.configAPI)
         .then((response) => {
            this.currencyPairs = response.data.currencyPairs;
            this.show = true;
        }).catch( error => { console.log(error); });
      },

      getRates: function(currencyPairsIDs) {
        var requestRatesIds =  currencyPairsIDs || Object.keys(this.currencyPairs);

        axios.get(this.exchangeRateServer + this.ratesAPI, {
            params: {
            currencyPairIds : requestRatesIds
           },
           responseType: 'json'
           })
          .then((response) => {
            var rates = response.data.rates;
            var id = 0;

            var trends = {};
            for (id in rates) {
              var oldValue = this.currenciesRates[id];
              var newValue = rates[id];
              if (oldValue === undefined || 
                  oldValue === rates[id] ) {                 
                trends[id] = 'stagnating';
             } else if (oldValue > newValue ) {
                trends[id] = 'declining';
             } else {
                 trends[id] = 'growing';
             }
            }
            this.currencyRatesTrends = trends;
            this.currenciesRates = rates;            

        }).catch( error => { console.log(error); });      
      },
      refreshSelectedRates: function() {
        this.getRates();
      },
      shownCurrencyPairs: function(id) {
        if (this.isExist(this.currencyPairsFiltered, id)) {
          return true;          
        }
        return false;
      },
      isExist : function(arr, item) {
        for(var i = 0; i < arr.length; i = i + 1) {
          if( arr[i] === item ) {
            return true;
          }
        }
        return false;
      }
    }

  });
  vueInstance1.init();
}

