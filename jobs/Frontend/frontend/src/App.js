import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from 'react-router-dom';
import Nav from './components/Nav';
import MovieDetail from './components/MovieDetail';
import MovieList from './components/MovieList';
// import SearchArea from './components/SearchArea';

function App() {
  return (
    <Router>
      <div className="App">
        <Nav />

        <Switch>
          <Route path={'/'} exact>
            <MovieList />
          </Route>
          <Route path={'/movie/:movieId'} exact component={MovieDetail} />
          <Redirect to={'/'} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
