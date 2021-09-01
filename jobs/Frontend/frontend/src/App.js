import './App.css';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from 'react-router-dom';
import NavBar from './components/NavBar';
import MovieDetail from './pages/MovieDetail';
import HomePage from './pages/HomePage';

function App() {
  return (
    <Router>
      <div className="App">
        <NavBar />

        <Switch>
          <Route path={'/'} exact>
            <HomePage />
          </Route>
          <Route path={'/movie/:movieId'} exact component={MovieDetail} />
          <Redirect to={'/'} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
