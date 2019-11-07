import * as React from 'react';
import { PairsSelector } from './PairsSelector';
import { RateList } from './RateList';
import { compareRates } from './compareRates';
import { useConfig } from './useConfig';
import { useRates } from './useRates';
import { useRatesSelect } from './useRatesSelect';

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
      <>
        <PairsSelector pairs={pairs} togglePair={togglePair} />
        <RateList pairs={selectedPairs} />
      </>
    );
  }
  if (loadingFailed) {
    return <div>Config loading failed</div>;
  }
  return <div>Loading config...</div>;
};
