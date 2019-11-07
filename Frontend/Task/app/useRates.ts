import { useCallback, useEffect, useState } from 'react';
import { fetchRates, Rates } from './dataFetching/fetchRates';
import { Config } from './dataFetching/fetchConfiguration';

export function useRates(
  config: Config | null,
  ratesUrl: string,
  refreshMilliseconds: number,
): [Rates, Rates] {
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
      const intervalId = setInterval(loadRates, refreshMilliseconds);
      return () => {
        clearInterval(intervalId);
      };
    }
  }, [config, loadRates, refreshMilliseconds]);

  return [currentRates, previousRates];
}
