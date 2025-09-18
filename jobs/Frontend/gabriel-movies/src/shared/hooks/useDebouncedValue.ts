import { useEffect, useState } from "react";

export function useDebouncedValue<T>(value: T): T {
  const [debounced, setDebounced] = useState<T>(value);

  useEffect(() => {
    const id = setTimeout(() => setDebounced(value), 400);
    return () => clearTimeout(id);
  }, [value]);

  return debounced;
}
