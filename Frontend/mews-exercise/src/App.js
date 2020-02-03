import React from "react";
import "./App.css";
import MovieSearch from "./components/MovieSearch";
import MovieDetail from "./components/MovieDetail";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { Provider } from "react-redux";
import { store } from "./Helpers/ReduxHelper";

function App() {
  return (
    <Provider store={store}>
      <Router>
        <Switch>
          <Route
            exact
            path="/"
            render={props => <MovieSearch {...props} />}
          ></Route>
          <Route
            path="/moviedetail"
            render={props => <MovieDetail {...props} />}
          ></Route>
        </Switch>
      </Router>
    </Provider>
  );
}

export default App;
