import "./index.scss";
import "semantic-ui-css/semantic.min.css";

import React from "react";
import { Provider } from "react-redux";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";

import store from "./store";
import Details from "./views/details/Details";
import Search from "./views/search/Search";

const App: React.FC = () => {
  return (
    <Provider store={store}>
      <Router>
        <Switch>
          <Route exact path="/" component={Search} />
          <Route path="/movie/:id(\d+)" component={Details} />
        </Switch>
      </Router>
    </Provider>
  );
};

export default App;
