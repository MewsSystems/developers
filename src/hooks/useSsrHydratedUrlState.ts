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
 * Pattern: "URL-Synced State with SSR Hydration and Seamless SPA Navigation"
 *
 * Custom hook to synchronize state with the URL search params, while supporting both
 * SSR hydration (initial HTML load) and SPA-like client navigation—preserving Tanstack Query's cache and user experience.
 *
 * ## How This Supports the SSR Hydration/Data Loading Pattern:
 *
 * In our frontend, data flows through three key phases:
 *
 * 1. **Initial Page Load (SSR Hydration):**
 *    - When users visit the page directly or reload, Next.js does full SSR:
 *      - The server fetches data and sends a complete HTML document.
 *      - Tanstack Query is hydrated with this initial data for a fast, SEO-friendly experience.
 *      - Search params are parsed from the URL to initialize state and cache.
 *
 * 2. **Client-Side Navigation (Flight):**
 *    - Navigating between pages (via `<Link>`, `router.push`, etc.) uses Next.js "Flight":
 *      - Only changed React components/fragments are sent (not a new HTML document).
 *      - Tanstack Query **cannot rehydrate from server data** in this case.
 *      - Data is (re-)fetched on the client, often showing a loading state unless already cached.
 *      - This hook ensures URL params and state stay in sync as you navigate, avoiding reloads and preserving client cache.
 *
 * 3. **Client Data Updates (JSON fetches):**
 *    - When users interact with the UI (search, pagination), only the relevant data is fetched as JSON (via Tanstack Query).
 *    - The page stays responsive and fast, only updating necessary parts.
 *    - State stays in sync with URL, so users can bookmark/share/searches or use browser navigation (Back/Forward).
 *
 * ## Why not just use router.push for URL updates?
 *
 * Next.js's router methods may not always guarantee the desired navigation mode:
 *   - Updating only search params (not the full route) can trigger a full page reload in some Next.js versions,
 *     which re-triggers SSR and loses client-side cache and smooth UX.
 *   - Using browser history methods (like pushState) ensures updates to the URL without triggering SSR or a full reload,
 *     letting us keep Tanstack Query's cache and maintain a seamless SPA experience.
 *   - See: https://github.com/vercel/next.js/issues/72383
 *
 * ## Summary
 *
 * This hook is designed to:
 *   - Keep UI state in sync with the URL (search params), so reloads, bookmarking, and direct links always "just work".
 *   - Enable the SSR hydration pattern for initial loads, and fast, seamless client-side navigation after that.
 *   - Avoid unnecessary reloads or cache loss when updating the URL, preserving both the instant UX and Tanstack Query's client-side cache.
 *   - Make SPA navigation and server hydration work *together*—so the app is fast, SEO-friendly, and consistent, no matter how you navigate.
 *
 * Monitor Next.js updates: If router.push becomes more configurable (controlling navigation mode), consider adopting it natively.
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
