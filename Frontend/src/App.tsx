import { useEffect } from 'react';
import {
  BrowserRouter as Router,
  Redirect,
  Route,
  Switch,
} from 'react-router-dom';
import { useAppDispatch } from './hooks';
import MovieDetail from './pages/MovieDetail';
import MovieSearch from './pages/MovieSearch';
import { fetchConfiguration } from './redux/configurationReducer';

function App() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    // prefetch the configuration
    dispatch(fetchConfiguration());
  });

  return (
    <Router>
      <Switch>
        <Route path="/movie/:movieId">
          <MovieDetail />
        </Route>
        <Route path="/" exact>
          <MovieSearch />
        </Route>
        <Redirect from="*" to="/" />
      </Switch>
    </Router>
  );
}

export default App;
