import { useEffect, useRef, useState } from 'react';

const useDebounceValue = <T>(value: T, initialValue = '', delay = 500) => {
  const [debouncedValue, setDebouncedValue] = useState<T | string>(
    initialValue,
  );
  const timerRef = useRef<NodeJS.Timeout>();

  useEffect(() => {
    timerRef.current = setTimeout(() => setDebouncedValue(value), delay);

    return () => {
      clearTimeout(timerRef.current);
    };
  }, [value, delay]);

  return debouncedValue;
};

export default useDebounceValue;
