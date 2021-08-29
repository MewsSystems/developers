import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link,
  NavLink,
  Redirect,
} from 'react-router-dom';
import MovieDetail from './components/MovieDetail';
import MovieList from './components/MovieList';

function App() {
  return (
    <Router>
      <div className="App">
        <div className="App">
          <nav>
            <NavLink to={'/'}>Search</NavLink>
          </nav>
          <Switch>
            <Route path={'/'} exact component={MovieList} />
            <Route path={'/movie/:movieId'} exact component={MovieDetail} />
            <Redirect to={'/'} />
          </Switch>
        </div>
      </div>
    </Router>
  );
}

export default App;
