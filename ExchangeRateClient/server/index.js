const { SEED } = require('./constants');

const Chance = require('chance');
const chance = new Chance(SEED);

const ratesGenerator = require('./ratesGenerator');
ratesGenerator.init(chance);

const express = require('express');
const server = express();

server.use((req, res, next) => {
    res.header('Access-Control-Allow-Origin', '*');
    next();
});

server.get('/configuration', (req, res) => {
    const appLoadTime = chance.float({ min: 3, max: 5 }) * 1000;

    setTimeout(() => {
        res.jsonp({
            currencyPairs: ratesGenerator.getCurrencyPairs(),
        });
    }, appLoadTime);
});

server.get('/rates', (req, res) => {
    res.jsonp({
        rates: ratesGenerator.getCurrentRates(),
    });
});

server.listen(3000, () => {
    console.log('Server is running on port 3000.');
});
