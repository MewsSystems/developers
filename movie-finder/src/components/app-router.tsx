import React from "react";
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import { SearchView } from "../views/search-view";
import { MovieDetailView } from "../views/movie-detail-view";

export enum Routes {
  HOME = "/",
  SEARCH = HOME,
  DETAIL = "/movie-detail/{:movieId}",
}

export const AppRouter: React.FC = () => {
  return (
    <>
      <Router>
        <Switch>
          <Route exact path={Routes.SEARCH} component={SearchView} />
          <Route exact path={Routes.DETAIL} component={MovieDetailView} />
          <Redirect to={Routes.HOME} />
        </Switch>
      </Router>
    </>
  );
};
