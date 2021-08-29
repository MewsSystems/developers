import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  NavLink,
  Redirect,
} from 'react-router-dom';
import MovieDetail from './components/MovieDetail';
import MovieList from './components/MovieList';
// import SearchArea from './components/SearchArea';

function App() {
  return (
    <Router>
      <div className="App">
        <div className="App">
          <nav>
            <p>MOVIE</p>
            <NavLink to={'/'}>Search</NavLink>
          </nav>
          <Switch>
            <Route path={'/'} exact>
              <MovieList />
            </Route>
            <Route path={'/movie/:movieId'} exact component={MovieDetail} />
            <Redirect to={'/'} />
          </Switch>
        </div>
      </div>
    </Router>
  );
}

export default App;
