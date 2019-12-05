const {
  SEED,
  PAIR_COUNT,
  UPDATE_INTERVAL,
  FAILURE_CHANCE,
} = require ('../constants');

const path = require('path')
const Chance = require ('chance');
const chance = new Chance (SEED);
const PORT = 3087;
 
const ratesGenerator = require ('../ratesGenerator') ({
  generator: chance,
  pairCount: PAIR_COUNT,
  updateInterval: UPDATE_INTERVAL,
});

const express = require ('express');
const proxy = require('express-http-proxy');
const server = express ();

const apiRouter = express.Router()

server.use ((req, res, next) => {
  res.header ('Access-Control-Allow-Origin', '*');
  next ();
});

apiRouter.get ('/configuration', (req, res) => {
  const appLoadTime = chance.integer ({min: 1000, max: 1001});
  setTimeout (() => {
    res.json ({
      currencyPairs: ratesGenerator.getCurrencyPairs (),
    });
  }, appLoadTime);
});

apiRouter.get ('/rates', (req, res) => {
  const hasFailed = chance.floating ({min: 0, max: 1}) < FAILURE_CHANCE;

  if (hasFailed) return res.sendStatus (500);
  
  try {
    const allRates = ratesGenerator.getCurrentRates ();
    const {currencyPairIds = ''} = req.query;

    let rates = {};
    for (let pairId of currencyPairIds.split (',')) {
      if (typeof allRates[pairId] !== 'undefined') {
        rates[pairId] = allRates[pairId];
      }
    }
    res.json ({rates});
  } catch (e) {
    res.sendStatus (400);
  }
});

server.use('/api', apiRouter)


if (process.env.NODE_ENV === 'development') {
  /**
   * If we are developing then we proxy the fe requests 
   * to server started by react-scripts
   */
  server.use(proxy('http://localhost:3000'));
} else {
  /**
   * If we are in production we serve build folder straight away
   */
  const bundleDirectory = path.join(__dirname, '../../client/build');
  console.log(bundleDirectory)
  server.use('/', express.static(bundleDirectory))
}

server.listen (PORT, () => console.log (`Server is running on port ${PORT}`));