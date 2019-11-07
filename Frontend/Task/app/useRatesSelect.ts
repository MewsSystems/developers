import { useCallback, useState } from 'react';

export function useRatesSelect(): [
  ReadonlyArray<string>,
  (id: string) => void,
] {
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

  return [selectedPairIds, togglePair];
}
