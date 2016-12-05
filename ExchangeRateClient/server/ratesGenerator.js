const adjustDecimal = require('./misc/adjustDecimal');

let state = {};

module.exports = {
    init({ generator, pairCount, updateInterval }) {
        state.generator = generator;
        state.currencyPairs = generateCurrencyPairs(pairCount);

        // generateRates?
        setInterval(updateRates, updateInterval);
    },

    getCurrencyPairs() {
        return Object.assign({}, state.currencyPairs);
    },

    getCurrentRates() {
        return Object.assing({}, state.currentRates);
    },
};

function generateCurrencyPairs() {
    let rates = {};

    _.times(PAIRS_COUNT, () => {
        rates[chance.guid()] = chance.currency_pair();
    });

    return rates;
}

function generateRates() {
    
}

function updateRates() {

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
