const { SEED, PAIR_COUNT, UPDATE_INTERVAL, FAILURE_CHANCE } = require('./constants');
const Chance = require('chance');
const chance = new Chance(SEED);
const env = process.env;

const ratesGenerator = require('./ratesGenerator')({
    generator: chance,
    pairCount: PAIR_COUNT,
    updateInterval: UPDATE_INTERVAL,
});

const express = require('express');
const server = express();

server.use((req, res, next) => {
    res.header('Access-Control-Allow-Origin', '*');
    next();
});

server.get('/configuration', (req, res) => {
    const appLoadTime = env.DEBUG ? 0 : chance.integer({ min: 3000, max: 5000 });

    setTimeout(() => {
        res.json({
            currencyPairs: ratesGenerator.getCurrencyPairs(),
        });
    }, appLoadTime);
});

server.get('/rates', (req, res) => {
    const hasFailed = env.DEBUG ? false : chance.floating({ min: 0, max: 1 }) < FAILURE_CHANCE;

    if (hasFailed) {
        res.sendStatus(500);
    } else {
        try {
            const allRates = ratesGenerator.getCurrentRates();
            const { currencyPairIds = [] } = req.query;

            let rates = {};
            for (let pairId of currencyPairIds) {
                if (typeof allRates[pairId] !== 'undefined') {
                    rates[pairId] = allRates[pairId];
                }
            }

            res.json({ rates });
        } catch (e) {
            res.sendStatus(400);
        }
    }
});

server.listen(3001, () => {
    console.log('Server is running on port 3001.');
});
