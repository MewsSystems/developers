import * as React from 'react';
import { useCallback, useEffect, useState } from 'react';
import { Config, fetchConfiguration } from './dataFetching/fetchConfiguration';
import { PairsSelector } from './PairsSelector';

type Props = {
  configUrl: string;
};

export const Main = ({ configUrl }: Props) => {
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

  if (config) {
    const pairs = Object.entries(config.currencyPairs).map(([id, pair]) => ({
      id,
      currencies: pair,
      selected: selectedPairIds.includes(id),
    }));

    return (
      <>
        <PairsSelector pairs={pairs} togglePair={togglePair} />
      </>
    );
  }
  if (loadingFailed) {
    return <div>Config loading failed</div>;
  }
  return <div>Loading config...</div>;
};
