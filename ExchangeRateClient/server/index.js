const express import 'express';
const ratesGenerator import './ratesGenerator';

ratesGenerator.init();
const server = express();

server.get('/rates', (req, res) => {
    res.header('Access-Control-Allow-Origin', '*');
    res.jsonp({
        rates: ratesGenerator.getCurrentRates(),
    });
});

server.get('/configuration', (req, res) => {
    res.header('Access-Control-Allow-Origin', '*');
    res.jsonp({

    })
});

server.listen(3000, () => {
    console.log('Server is running on port 3000.');
});
