import nock from 'nock';
import { fetchRates } from './fetchRates';

it('fetches rates', async () => {
  const validResponse = {
    rates: {
      pair1: 1,
      pair2: 0.5,
    },
  };
  nock('http://example.com')
    .get('/rates?currencyPairIds[]=pair1&currencyPairIds[]=pair2')
    .reply(200, validResponse);

  const fetchedRates = await fetchRates('http://example.com/rates', [
    'pair1',
    'pair2',
  ]);
  expect(fetchedRates).toEqual({
    pair1: 1,
    pair2: 0.5,
  });
});

it('rejects invalid response', async () => {
  const invalidResponse = {
    rates: {
      pair1: 'string instead of a number',
      pair2: 0.5,
    },
  };
  nock('http://example.com')
    .get('/rates?currencyPairIds[]=pair1&currencyPairIds[]=pair2')
    .reply(200, invalidResponse);

  const ratesPromise = fetchRates('http://example.com/rates', [
    'pair1',
    'pair2',
  ]);
  await expect(ratesPromise).rejects.toBeTruthy();
});
