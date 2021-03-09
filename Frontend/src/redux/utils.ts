import { AnyAction } from 'redux';

export const isActionAborted = (action: AnyAction) =>
  action.meta.aborted || false;

export const updateState = <T>(target: T, source: Partial<T>) => {
  const targetKeys = Object.keys(target) as (keyof T)[];

  targetKeys.forEach((key) => {
    if (source[key] !== undefined) {
      target[key] = source[key] as T[keyof T];
    }
  });
};
