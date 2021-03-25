import {
  BrowserRouter as Router,
  Redirect,
  Route,
  Switch,
} from 'react-router-dom';
import { QueryParamProvider } from 'use-query-params';
import Layout from './components/Layout';
import SearchInput from './components/SearchInput';
import SearchResults from './components/SearchResults';
import MovieDetail from './components/MovieDetail';
import Flex from './components/common/Flex';
import Container from './components/common/Container';
import BackToSearchResults from './components/BackButton/BackToSearchResults';
import { ErrorBoundary } from 'react-error-boundary';
import ErrorFallback from './components/ErrorFallback';

const SEARCH_RESULTS_PATH = '/';

const Header = () => {
  return (
    <Container padding="0">
      <Flex flexWrap="nowrap" gap="2rem">
        <BackToSearchResults searchResultsPath={SEARCH_RESULTS_PATH} />
        <QueryParamProvider ReactRouterRoute={Route}>
          <SearchInput
            maxWidth="900px"
            placeholderText="Search Movies"
            resultsPath={SEARCH_RESULTS_PATH}
          />
        </QueryParamProvider>
      </Flex>
    </Container>
  );
};

function App() {
  return (
    <Router>
      <Layout header={<Header />}>
        <ErrorBoundary FallbackComponent={ErrorFallback}>
          <Switch>
            <Route path="/movie/:movieId" exact>
              <MovieDetail />
            </Route>
            <Route path={SEARCH_RESULTS_PATH} exact>
              <QueryParamProvider ReactRouterRoute={Route}>
                <SearchResults />
              </QueryParamProvider>
            </Route>
            <Redirect from="*" to={SEARCH_RESULTS_PATH} />
          </Switch>
        </ErrorBoundary>
      </Layout>
    </Router>
  );
}

export default App;
