import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import * as React from 'react';
import { ErrorBoundary } from 'react-error-boundary';

import { queryConfig } from './api/lib/react-query-config.ts';
import { createContext } from 'react';
import Spinner from '../components/spinner';

type AppProviderProps = {
  children: React.ReactNode;
};

export type GlobalSearch = {
  searchQuery: string;
  setSearchQuery: (c: string) => void;
};

export const GlobalSearchContext = createContext<GlobalSearch>({
  searchQuery: '',
  setSearchQuery: () => {},
});

export const AppProvider = ({ children }: AppProviderProps) => {
  const [queryClient] = React.useState(
    () =>
      new QueryClient({
        defaultOptions: queryConfig,
      }),
  );

  const [searchQuery, setSearchQuery] = React.useState('');

  return (
    <React.Suspense fallback={<Spinner></Spinner>}>
      <ErrorBoundary fallback={<div>Something went wrong</div>}>
        <QueryClientProvider client={queryClient}>
          <GlobalSearchContext.Provider value={{ searchQuery, setSearchQuery }}>
            {import.meta.env.DEV && <ReactQueryDevtools />}
            {children}
          </GlobalSearchContext.Provider>
        </QueryClientProvider>
      </ErrorBoundary>
    </React.Suspense>
  );
};
