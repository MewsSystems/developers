import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import * as React from 'react';
import { ErrorBoundary } from 'react-error-boundary';

import { queryConfig } from './api/lib/react-query-config.ts';
import { createContext } from 'react';
import Spinner from '../components/spinner';
import { theme, ThemeColors } from '../assets/colors/theme/theme.ts';
import { ThemeProvider } from 'styled-components';

type AppProviderProps = {
  children: React.ReactNode;
};

export type AppContext = {
  searchQuery: string;
  setSearchQuery: (c: string) => void;
  themeColor: ThemeColors;
  setThemeColor: (c: ThemeColors) => void;
};

export const GlobalContext = createContext<AppContext>({
  searchQuery: '',
  setSearchQuery: () => {},
  themeColor: 'blue',
  setThemeColor: () => {},
});

export const AppProvider = ({ children }: AppProviderProps) => {
  const [queryClient] = React.useState(
    () =>
      new QueryClient({
        defaultOptions: queryConfig,
      }),
  );

  const [searchQuery, setSearchQuery] = React.useState('');
  const [themeColor, setThemeColor] = React.useState<ThemeColors>('blue');

  return (
    <React.Suspense fallback={<Spinner></Spinner>}>
      <ErrorBoundary fallback={<div>Something went wrong</div>}>
        <QueryClientProvider client={queryClient}>
          <GlobalContext.Provider value={{ searchQuery, setSearchQuery, themeColor, setThemeColor }}>
            <ThemeProvider theme={theme[themeColor]}>
              {import.meta.env.DEV && <ReactQueryDevtools />}
              {children}
            </ThemeProvider>
          </GlobalContext.Provider>
        </QueryClientProvider>
      </ErrorBoundary>
    </React.Suspense>
  );
};
