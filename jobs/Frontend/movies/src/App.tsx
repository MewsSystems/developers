import SearchMovies from "./pages/SearchMovies";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import ErrorBoundary from "./components/ErrorBoundary";
import ErrorFallback from "./components/ErrorFallback";
import PageContainer from "./components/PageContainer";

const queryClient = new QueryClient();

function App() {
  return (
    <PageContainer>
      <ErrorBoundary fallbackComponent={ErrorFallback}>
        <QueryClientProvider client={queryClient}>
          <SearchMovies />
        </QueryClientProvider>
      </ErrorBoundary>
    </PageContainer>
  );
}

export default App;
