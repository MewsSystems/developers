import { DefaultOptions } from '@tanstack/react-query';

export const queryConfig = {
  queries: {
    // throwOnError: true,
    refetchOnWindowFocus: false,
    retry: true,
    staleTime: 1000 * 60 * 10,
  },
} satisfies DefaultOptions;

export type QueryConfig<T extends (...args: any[]) => any> = Omit<ReturnType<T>, 'queryKey' | 'queryFn'>;
