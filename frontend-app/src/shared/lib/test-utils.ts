/**
 * Test utilities for React Query and other testing needs
 */
import { QueryClient } from '@tanstack/react-query';

/**
 * Creates a test query client with disabled retries and caching for testing
 */
export const createTestQueryClient = (): QueryClient => {
  return new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
        gcTime: 0,
        staleTime: 0,
      },
      mutations: {
        retry: false,
      },
    },
  });
};