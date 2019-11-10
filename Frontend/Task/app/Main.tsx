import * as React from 'react';
import { PairsSelector } from './PairsSelector';
import { RateList } from './RateList';
import { compareRates } from './compareRates';
import { useConfig } from './useConfig';
import { useRates } from './useRates';
import { useRatesSelect } from './useRatesSelect';
import styled from 'styled-components';

const Layout = styled.div`
  max-width: 500px;
  @media (min-width: 550px) {
    margin: 100px auto;
  }
`;

type Props = {
  configUrl: string;
  ratesUrl: string;
  ratesRefreshMilliseconds: number;
};

export const Main = ({
  configUrl,
  ratesRefreshMilliseconds,
  ratesUrl,
}: Props) => {
  const [config, loadingFailed] = useConfig(configUrl);
  const [selectedPairIds, togglePair] = useRatesSelect();
  const [currentRates, previousRates] = useRates(
    config,
    ratesUrl,
    ratesRefreshMilliseconds,
  );

  if (config) {
    const pairs = Object.entries(config.currencyPairs).map(([id, pair]) => ({
      id,
      currencies: pair,
      selected: selectedPairIds.includes(id),
      rate: currentRates[id],
      trend: compareRates(currentRates[id], previousRates[id]),
    }));
    const selectedPairs = pairs.filter(pair => pair.selected);

    return (
      <Layout>
        <PairsSelector pairs={pairs} togglePair={togglePair} />
        <RateList pairs={selectedPairs} />
      </Layout>
    );
  }
  if (loadingFailed) {
    return <Layout>Config loading failed</Layout>;
  }
  return <Layout>Loading config...</Layout>;
};
