import { useCallback, useMemo } from 'react';
import { useSearchParams } from 'react-router-dom';

const Q = 'q';

const useQuerySearch = () => {
  const [searchParams, setSearchParams] = useSearchParams();

  const querySearch = useMemo(
    () => searchParams.get(Q) ?? undefined,
    [searchParams],
  );

  const setQuerySearch = useCallback(
    (value: string) => {
      setSearchParams(value ? { [Q]: value } : undefined);
    },
    [setSearchParams],
  );

  return {
    querySearch,
    setQuerySearch,
  };
};

export default useQuerySearch;
