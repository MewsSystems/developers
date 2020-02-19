import React from 'react';
import './App.css';
import { Provider } from 'react-redux';
import { store } from './store/Store';
import { Route, Switch, Redirect } from 'react-router-dom';
import { ConnectedRouter } from 'connected-react-router';
import Movies from './movies/Movies';
import { history } from './store/Store';

const App=()=>(
  <Provider store={ store }>
    <ConnectedRouter history={history}>
      <Switch>
        <Route
          exact
          path="/"
          component={ Movies }
        />
        <Route
          path="/:movieId"
          component={ Movies }
        />

        {/*<Route component={ FourOhFour } />*/}
      </Switch>
    </ConnectedRouter>
  </Provider>
)

export default App;
