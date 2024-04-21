import React from "react";
import { Route, Switch } from "wouter";
import { MoviesDetailView } from "./views/MoviesDetailView";
import { MovieListView } from "./views/MovieListView";
import { Layout } from "@components/ui/Layout";

function App() {
  return (
    <Layout>
      <Switch>
        <Route path="/" component={() => <MovieListView />} />
        <Route path="/movies/:id" component={MoviesDetailView} />
      </Switch>
    </Layout>
  );
}

export default App;
