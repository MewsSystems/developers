const {
  MIN_INITIAL_RATE,
  MAX_INITIAL_RATE,
  MAX_RATE_UPDATE_STEP,
} = require('./constants');

module.exports = function rateGenerator({ generator, pairCount, updateInterval }) {
  const currencyPairs = generateCurrencyPairs();
  let currentRates = generateRates(currencyPairs);
  setInterval(() => (currentRates = updateRates(currentRates)), updateInterval);

  return {
    getCurrencyPairs() {
      return Object.assign({}, currencyPairs);
    },

    getCurrentRates() {
      return Object.assign({}, currentRates);
    },
  };

  function generateCurrencyPairs() {
    const pairs = {};

    for (let i = 0; i != pairCount; ++i) {
      pairs[generator.guid()] = generator.currency_pair();
    }

    return pairs;
  }

  function generateRates(currencyPairs) {
    const rates = {};

    for (const pairId of Object.keys(currencyPairs)) {
      rates[pairId] = generator.floating({ min: MIN_INITIAL_RATE, max: MAX_INITIAL_RATE });
    }

    return rates;
  }

  function updateRates(currentRates) {
    const updatedRates = {};

    for (const pairId of Object.keys(currentRates)) {
      const value = currentRates[pairId];
      updatedRates[pairId] = generator.floating({ min: value + MAX_RATE_UPDATE_STEP, max: value + MAX_RATE_UPDATE_STEP });
    }

    return updatedRates;
  }
};
