import * as React from 'react';

export const useDebounce = (ms: number) => {
  let ref = React.useRef<NodeJS.Timeout>();

  const debounce = React.useCallback(
    (func: () => void) => {
      if (ref.current !== undefined) clearTimeout(ref.current);

      ref.current = setTimeout(func, ms);
    },
    [ref],
  );

  return debounce;
};
