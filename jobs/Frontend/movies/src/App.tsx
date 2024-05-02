import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Route, Switch, Redirect } from "wouter";
import SearchMovies from "./pages/SearchMovies";
import MovieDetail from "./pages/MovieDetail";
import ErrorBoundary from "./components/ErrorBoundary";
import ErrorFallback from "./components/ErrorFallback";
import PageContainer from "./components/PageContainer";

const queryClient = new QueryClient();

function App() {
  return (
    <PageContainer>
      <ErrorBoundary fallbackComponent={ErrorFallback}>
        <QueryClientProvider client={queryClient}>
          <Switch>
            <Route path="/movies/:id">
              {(params) => <MovieDetail id={params.id} />}
            </Route>
            <Route path="/" component={SearchMovies} />
            <Route path="*">
              <Redirect to="/" />
            </Route>
          </Switch>
        </QueryClientProvider>
      </ErrorBoundary>
    </PageContainer>
  );
}

export default App;
