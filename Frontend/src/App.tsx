import {
  BrowserRouter as Router,
  Redirect,
  Route,
  Switch,
  useRouteMatch,
} from 'react-router-dom';
import { QueryParamProvider } from 'use-query-params';
import Layout from './components/Layout';
import SearchInput from './components/SearchInput';
import SearchResults from './components/SearchResults';
import MovieDetail from './components/MovieDetail';
import BackButton from './components/BackButton';
import Flex from './components/common/Flex';
import Container from './components/common/Container';

const SEARCH_RESULTS_PATH = '/';

const Header = () => {
  const match = useRouteMatch({ path: SEARCH_RESULTS_PATH, exact: true });

  return (
    <Container padding="0">
      <Flex flexWrap="nowrap" gap="2rem">
        {!match && <BackButton />}
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
      </Layout>
    </Router>
  );
}

export default App;
