import React from "react";
import styled from "styled-components";
import { Route } from "wouter";
import { MoviesDetailView } from "./MoviesDetailView";
import { MovieListView } from "./MovieList";

function App() {
  return (
    <MainLayout>
      {/*  Movies View */}
      <Route path="/" component={MovieListView} />
      {/*  Details Movie View */}
      <Route path="/movies/:id" component={MoviesDetailView} />
    </MainLayout>
  );
}

export default App;

const MainLayout = styled.main`
  display: flex;
  background-color: #77b0aa;
  flex-direction: column;
  font-family: "Poppins", sans-serif;
  font-weight: 800;
`;
