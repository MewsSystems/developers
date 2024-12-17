import { useRef } from "react";

const useDebounce = <ArgsType extends unknown[]>(
  // eslint-disable-next-line @typescript-eslint/no-unsafe-function-type
  callback: Function,
  ms: number = 0
) => {
  const timeoutId = useRef<NodeJS.Timeout | null>(null);
  return (...args: ArgsType) => {
    if (timeoutId.current) {
      clearTimeout(timeoutId.current);
    }

    timeoutId.current = setTimeout(() => {
      callback(...args);
    }, ms);
  };
};

export default useDebounce;
