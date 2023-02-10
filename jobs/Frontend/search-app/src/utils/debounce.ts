let debouncedFn: NodeJS.Timeout | undefined = undefined;

export const debounce = <T>(fn: (value: T) => void, delay: number) => {
  return function (value: T) {
    clearTimeout(debouncedFn);
    debouncedFn = setTimeout(() => fn(value), delay);
  };
};
