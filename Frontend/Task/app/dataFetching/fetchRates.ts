import fetch from 'isomorphic-fetch';
import * as t from 'io-ts';
import { stringify as queryStringStringify } from 'query-string';

const ratesCodec = t.record(t.string, t.number);

const responseCodec = t.interface({
  rates: ratesCodec,
});

export type Rates = t.TypeOf<typeof ratesCodec>;

export async function fetchRates(
  url: string,
  currencyPairIds: ReadonlyArray<string>,
) {
  const queryString = queryStringStringify({
    'currencyPairIds[]': currencyPairIds,
  });
  const response = await fetch(`${url}?${queryString}`);
  const body = await response.json();

  const rates = responseCodec.decode(body);

  if (rates._tag === 'Right') {
    return rates.right.rates;
  }
  throw new Error('Rates response is invalid');
}
