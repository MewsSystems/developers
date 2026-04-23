import { Dispatch, SetStateAction, useState, useEffect } from "react";

function useDebouncedState<S>(
  initialState: S | (() => S),
  delay: number = 200
): [S, Dispatch<SetStateAction<S>>, boolean] {
  const [value, setValue] = useState<S>(initialState);
  const [debouncedValue, setDebouncedValue] = useState<S>(initialState);

  useEffect(() => {
    const timeoutID = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => {
      clearTimeout(timeoutID);
    };
  }, [value, delay]);

  const isChanged = value !== debouncedValue;

  return [debouncedValue, setValue, isChanged];
}


export default useDebouncedState;