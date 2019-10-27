import nock from 'nock';
import { fetchConfiguration } from './fetchConfiguration';

it('fetches the configuration', async () => {
  const validConfig = {
    currencyPairs: {
      'f70c6744-c2cb-5a28-b4c6-5aa0680dac0c': [
        { code: 'MZN', name: 'Mozambique Metical' },
        { code: 'GHS', name: 'Ghana Cedi' },
      ],
      '43a1ae34-1cae-50fd-bb74-d13042a845c2': [
        { code: 'BAM', name: 'Bosnia and Herzegovina Convertible Marka' },
        { code: 'ALL', name: 'Albania Lek' },
      ],
    },
  };

  nock('http://example.com')
    .get('/config')
    .reply(200, validConfig);

  const fetchedConfiguration = await fetchConfiguration(
    'http://example.com/config',
  );
  expect(fetchedConfiguration).toEqual(validConfig);
});

it('fails on invalid configuration', async () => {
  const invalidConfig = {
    currencyPairs: {
      'f70c6744-c2cb-5a28-b4c6-5aa0680dac0c': [
        { name: 'Mozambique Metical' }, // missing code
        { code: 'GHS', name: 'Ghana Cedi' },
      ],
    },
  };

  nock('http://example.com')
    .get('/config')
    .reply(200, invalidConfig);

  await expect(
    fetchConfiguration('http://example.com/config'),
  ).rejects.toBeTruthy();
});

it('fails on network error', async () => {
  nock('http://example.com')
    .get('/config')
    .replyWithError('error');

  await expect(
    fetchConfiguration('http://example.com/config'),
  ).rejects.toBeTruthy();
});
