import { useCallback, useEffect, useState } from 'react';

type ParamParsers<T extends object> = {
  [K in keyof T]?: (val: string | null) => T[K];
};

type ShouldOmitParam<T extends object> = Partial<{
  [K in keyof T]: (value: T[K]) => boolean;
}>;

type ParamsConfig<T extends object> = {
  initialParams: T;
  paramParse?: ParamParsers<T>;
  shouldOmitParam?: ShouldOmitParam<T>;
};

/**
 * Pattern: "URL-Synced State with SSR Hydration and SPA History"
 *
 * Custom hook for SSR-safe search param state with SPA-style updates.
 * This helps support the Tanstack Query hydration pattern we are using in the frontend.
 *
 * In Next.js 15, router.push() may cause a hard navigation (full reload) when updating only search params.
 * See issue: https://github.com/vercel/next.js/issues/72383
 *
 * This hook uses window.history.pushState for guaranteed "soft" navigation and React Query cache preservation.
 * Note: Monitor Next.js releases—if router.push becomes fully configurable for soft navigation, consider adopting it.
 */
export function useSsrHydratedUrlState<T extends object>({
  initialParams,
  paramParse = {},
  shouldOmitParam = {},
}: ParamsConfig<T>) {
  const getParamsFromUrl = useCallback((): T => {
    if (typeof window === 'undefined') {
      return { ...initialParams };
    }
    const params = new URLSearchParams(window.location.search);
    const result = {} as T;

    (Object.keys(initialParams) as Array<keyof T>).forEach((key) => {
      const rawValue = params.get(key as string);
      if (paramParse[key]) {
        result[key] = paramParse[key]!(rawValue);
      } else {
        result[key] = (rawValue ?? initialParams[key]) as T[keyof T];
      }
    });

    return result;
  }, [initialParams, paramParse]);

  const [params, setParamsState] = useState<T>(getParamsFromUrl);

  // Listen for browser back/forward (popstate) to keep state in sync with URL.
  useEffect(() => {
    const handler = () => {
      const newParams = getParamsFromUrl();
      let changed = false;
      (Object.keys(newParams) as Array<keyof T>).forEach((key) => {
        if (newParams[key] !== params[key]) {
          changed = true;
        }
      });
      if (changed) setParamsState(newParams);
    };

    window.addEventListener('popstate', handler);

    return () => window.removeEventListener('popstate', handler);
  }, [params, getParamsFromUrl]);

  const setParams = useCallback(
    (newValues: Partial<T>) => {
      const urlParams = new URLSearchParams(window.location.search);
      (Object.entries({ ...params, ...newValues }) as [keyof T, T[keyof T]][]).forEach(
        ([key, value]) => {
          if (
            value === undefined ||
            value === '' ||
            value === null ||
            (shouldOmitParam?.[key] && shouldOmitParam[key]!(value))
          ) {
            urlParams.delete(key as string);
          } else {
            urlParams.set(key as string, String(value));
          }
        }
      );

      const newUrl = `${window.location.pathname}?${urlParams.toString()}`;
      window.history.pushState({}, '', newUrl);

      setParamsState((old) => ({ ...old, ...newValues }));
    },
    [params, shouldOmitParam]
  );

  const push = useCallback(
    (newValues: Partial<T>) => {
      const urlParams = new URLSearchParams();
      (Object.entries({ ...initialParams, ...newValues }) as [keyof T, T[keyof T]][]).forEach(
        ([key, value]) => {
          if (
            value !== undefined &&
            value !== '' &&
            value !== null &&
            !(shouldOmitParam?.[key] && shouldOmitParam[key]!(value))
          ) {
            urlParams.set(key as string, String(value));
          }
        }
      );

      const newUrl = `${window.location.pathname}?${urlParams.toString()}`;
      window.history.pushState({}, '', newUrl);

      setParamsState({ ...initialParams, ...newValues });
    },
    [initialParams, shouldOmitParam]
  );

  return {
    params,
    setParams,
    push,
  };
}
