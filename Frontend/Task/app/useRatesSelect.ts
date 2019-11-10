import { useCallback, useState } from 'react';
import {
  loadSelectedPairs,
  storeSelectedPairs,
} from './dataFetching/selectedPairs';

export function useRatesSelect(): [
  ReadonlyArray<string>,
  (id: string) => void,
] {
  const [selectedPairIds, setSelectedPairs] = useState<ReadonlyArray<string>>(
    loadSelectedPairs,
  );

  const togglePair = useCallback((togglingId: string) => {
    setSelectedPairs(selectedPairs => {
      if (selectedPairs.includes(togglingId)) {
        return storeSelectedPairs(
          selectedPairs.filter(selectedId => selectedId !== togglingId),
        );
      }
      return storeSelectedPairs([...selectedPairs, togglingId]);
    });
  }, []);

  return [selectedPairIds, togglePair];
}
