import fetch from 'isomorphic-fetch';
import * as t from 'io-ts';

const currencyCodec = t.interface({
  code: t.string,
  name: t.string,
});

const pairCodec = t.tuple([currencyCodec, currencyCodec]);

const configCodec = t.interface({
  currencyPairs: t.record(t.string, pairCodec),
});

export type Config = t.TypeOf<typeof configCodec>;

export async function fetchConfiguration(url: string) {
  const response = await fetch(url);
  const body = await response.json();

  const config = configCodec.decode(body);
  if (config._tag === 'Right') {
    return config.right;
  }
  throw new Error('Configuration is invalid');
}
