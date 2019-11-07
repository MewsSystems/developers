import * as React from 'react';
import { useCallback, useEffect, useState } from 'react';
import { Config, fetchConfiguration } from './dataFetching/fetchConfiguration';
import { PairsSelector } from './PairsSelector';
import { RateList } from './RateList';
import { fetchRates, Rates } from './dataFetching/fetchRates';
import { compareRates } from './compareRates';

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
  const [config, setConfig] = useState<Config | null>(null);
  const [loadingFailed, setLoadingFailed] = useState(false);
  const [selectedPairIds, setSelectedPairs] = useState<ReadonlyArray<string>>(
    [],
  );

  const togglePair = useCallback((togglingId: string) => {
    setSelectedPairs(selectedPairs => {
      if (selectedPairs.includes(togglingId)) {
        return selectedPairs.filter(selectedId => selectedId !== togglingId);
      }
      return [...selectedPairs, togglingId];
    });
  }, []);

  useEffect(() => {
    fetchConfiguration(configUrl)
      .then(setConfig)
      .catch(() => {
        setLoadingFailed(true);
      });
  }, [configUrl]);

  const [[currentRates, previousRates], setRates] = useState<[Rates, Rates]>([
    {},
    {},
  ]);

  const loadRates = useCallback(async () => {
    if (config) {
      const freshestRates = await fetchRates(
        ratesUrl,
        Object.keys(config.currencyPairs),
      );
      setRates(([formerCurrentRates, formerPreviousRates]) => [
        {
          ...formerCurrentRates,
          ...freshestRates,
        },
        {
          ...formerPreviousRates,
          ...formerCurrentRates,
        },
      ]);
    }
  }, [config, ratesUrl]);

  useEffect(() => {
    if (config) {
      loadRates();
      const intervalId = setInterval(loadRates, ratesRefreshMilliseconds);
      return () => {
        clearInterval(intervalId);
      };
    }
  }, [config, loadRates, ratesRefreshMilliseconds]);

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
