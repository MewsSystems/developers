import styled from "styled-components";
import SearchMovies from "./pages/SearchMovies";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import ErrorBoundary from "./components/ErrorBoundary";
import ErrorFallback from "./components/ErrorFallback";

const queryClient = new QueryClient();

function App() {
  return (
    <PageContainer>
      <ErrorBoundary fallbackComponent={ErrorFallback}>
        <h1>Something</h1>
        <QueryClientProvider client={queryClient}>
          <SearchMovies />
        </QueryClientProvider>
      </ErrorBoundary>
    </PageContainer>
  );
}

const PageContainer = styled.main`
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 2rem;
  text-align: center;
`;

export default App;
