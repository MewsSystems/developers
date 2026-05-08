import { ReactNode } from 'react';
import { QueryClient, QueryClientProvider, HydrationBoundary } from '@tanstack/react-query';
import { SearchProvider } from '../features/movies/context';

export default function Providers({ children }: { children: ReactNode }) {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <SearchProvider>
        <HydrationBoundary>{children}</HydrationBoundary>
      </SearchProvider>
    </QueryClientProvider>
  );
}
