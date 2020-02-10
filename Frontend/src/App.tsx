import React from 'react';
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";

import { reducers } from './reducers';
import { createStore, applyMiddleware } from 'redux';
import { Provider } from 'react-redux';
import thunk from 'redux-thunk';
import { MovieView } from './views/detail';
import { SearchView } from './views/search';
import { Layout, CenterColumn } from './components';



const store = createStore(reducers, applyMiddleware(thunk));

const App = () => {
  return (
    <Provider store={store} >
      <Router>
        <Switch>
          <Layout>
            <CenterColumn>
              <Route exact path="/" component={SearchView} />
              <Route path="/movie/:id" component={MovieView} />
            </CenterColumn>
          </Layout>
        </Switch>
      </Router>
    </Provider>
  );
}

export default App;
