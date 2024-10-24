import { useRef, useEffect } from 'react';

type Timer = ReturnType<typeof setTimeout>;

const useDebounce = (delayedFunc: (arg: any) => void, delay = 1000) => {
  const timer = useRef<Timer>();

  useEffect(() => {
    return () => {
      if (!timer.current) return;
      clearTimeout(timer.current);
    };
  }, []);

  const debouncedFunction = (arg: any) => {
    const newTimer = setTimeout(() => {
      delayedFunc(arg);
    }, delay);
    clearTimeout(timer.current);
    timer.current = newTimer;
  };

  return debouncedFunction;
};

export default useDebounce;
