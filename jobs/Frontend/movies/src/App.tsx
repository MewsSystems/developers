import styled from "styled-components";
import SearchMovies from "./pages/SearchMovies";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

const queryClient = new QueryClient();

function App() {
  return (
    <PageContainer>
      <h1>Something</h1>
      <QueryClientProvider client={queryClient}>
        <SearchMovies />
      </QueryClientProvider>
    </PageContainer>
  );
}

const PageContainer = styled.main`
  max-width: 1280px;
  margin: 0 auto;
  padding: 2rem;
  text-align: center;
`;

export default App;
