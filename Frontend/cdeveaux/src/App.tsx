import React from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from 'react-router-dom';

import Search from './screens/Search';
import Detail from './screens/Detail';

const App = () => {
  return (
    <Router>
      <Switch>
        <Route path="/" exact component={Search}/>
        <Route path="/:assetId" exact component={Detail}/>
      </Switch>
    </Router>
  );
}

export default App;
