// @flow strict

import { useEffect, useCallback } from 'react';

const useInterval = (callback: () => void, interval: number) => {
  const memoizedCallback = useCallback(() => {
    callback();
  }, [callback]);

  useEffect(() => {
    const intervalId = setInterval(memoizedCallback, interval);
    return () => clearInterval(intervalId);
  });
};

export default useInterval;
