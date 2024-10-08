import { MovieSearchProvider } from "./MovieSearchProvider"
import QueryClientProvider from "./QueryClientProvider"

const Providers = ({ children }: { children: React.ReactNode }) => {
  return (
    <QueryClientProvider>
      <MovieSearchProvider>{children}</MovieSearchProvider>
    </QueryClientProvider>
  )
}

export default Providers
