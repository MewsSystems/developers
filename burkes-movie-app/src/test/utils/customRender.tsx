import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { render } from '@testing-library/react';
import { ReactNode } from 'react';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
    },
  },
});

export const customRender = (component: ReactNode) =>
  render(
    <QueryClientProvider client={queryClient}>{component}</QueryClientProvider>
  );
