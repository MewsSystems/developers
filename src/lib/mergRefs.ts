import { Ref, RefCallback, RefObject } from 'react';

export function mergeRefs<T>(...args: (Ref<T> | undefined)[]): RefCallback<T> {
  return (value: T) => {
    args.forEach((ref) => {
      if (typeof ref === 'function') {
        ref(value);
      } else if (ref && typeof ref === 'object') {
        (ref as RefObject<T>).current = value;
      }
    });
  };
}
