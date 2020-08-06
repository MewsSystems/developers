import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import Search from './Search/Search';
import Movie from './Movie/Movie';
import { Provider } from 'react-redux';
import createStore from '../store';
import './App.scss';

const store = createStore();

const App = () => {
  return (
    <Provider store={store}>
      <Router>
        <Switch>
          <Route path="/" exact component={Search} />
          <Route path="/movie/:id" render={({ match }) => <Movie id={match.params.id} />} />
        </Switch>
      </Router>
    </Provider>
  );
};

export default App;
