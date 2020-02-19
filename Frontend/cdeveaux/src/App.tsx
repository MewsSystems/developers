import React from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from 'react-router-dom';

import Search from './screens/Search/index';

console.error(Search);
const App = () => {
  return (
    <Router>
      <Switch>
        <Route path="/" exact component={Search}/>
        <Route path="/:assetId">

        </Route>
      </Switch>
    </Router>
  );
}

export default App;
