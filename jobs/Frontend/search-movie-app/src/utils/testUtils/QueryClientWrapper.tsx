import type { ReactNode } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

export const QueryClientWrapper = ({ children }: { children: ReactNode }) => {
  const testQueryClient = new QueryClient({
    defaultOptions: {
      queries: { retry: false },
    },
  });
  return <QueryClientProvider client={testQueryClient}>{children}</QueryClientProvider>;
};
