var pairs = require('./pairs.json');
var rates = {};
var UPDATE_INTERVAL = 1000;

function adjustDecimal(value, exp) {
  value = value.toString().split('e');
  value = Math.floor(+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
  value = value.toString().split('e');
  return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
}

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function generateRate(pair) {
    rates[pair] = adjustDecimal(getRandomInt(500, 1500) / 1000, -3);
}

function updateRate(pair) {
    rates[pair] = adjustDecimal(rates[pair] + getRandomInt(-5, 5) / 1000, -3);
}

function updateRates() {
    pairs.forEach(updateRate)
}

module.exports = {
    init() {
        pairs.forEach(generateRate);
        setInterval(updateRates, UPDATE_INTERVAL);
    },

    getCurrentRates() {
        return rates;
    }
}
