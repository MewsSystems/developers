import { QueryClient, type DefaultOptions } from '@tanstack/react-query'

const defaultQueryConfig = {
  queries: { refetchOnWindowFocus: false },
} satisfies DefaultOptions

export const queryClient = new QueryClient({
  defaultOptions: defaultQueryConfig,
})
