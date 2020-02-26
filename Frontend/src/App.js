import React, { useEffect } from 'react';
import { Provider } from 'react-redux';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import store from './store';
import Home from './pages/Home';
import Detail from './pages/Detail';
import NavBar from './components/layout/NavBar';
import About from './pages/About';
import NotFound from './pages/NotFound';

import 'materialize-css/dist/css/materialize.min.css';
import M from 'materialize-css/dist/js/materialize.min';
import './App.css';


const App = () => {

  useEffect(() => {
    M.AutoInit();
  });

  return (
    <Provider store={store}>
      <Router>
        <NavBar />
        <Switch>
          <Route exact path='/' component={Home} />
          <Route exact path='/about' component={About} />
          <Route exact path='/movie/:id' component={Detail} />
          <Route component={NotFound} />
        </Switch>
      </Router>
    </Provider>
  );
};

export default App;
