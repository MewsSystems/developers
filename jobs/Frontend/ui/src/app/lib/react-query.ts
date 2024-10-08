import { QueryClient, DefaultOptions } from "@tanstack/react-query"

const queryConfig: DefaultOptions = {
  queries: {
    retry: false,
  },
}

export const queryClient = new QueryClient({
  defaultOptions: queryConfig,
})
