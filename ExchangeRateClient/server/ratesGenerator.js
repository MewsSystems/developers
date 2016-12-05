import Chance from 'Chance';
import pairs from './pairs';
import {
    SEED,
    UPDATE_INTERVAL,
    
} from './constants';
import adjustDecimal from './misc/adjustDecimal';

const chance = new Chance(SEED);

let rates = {};

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
