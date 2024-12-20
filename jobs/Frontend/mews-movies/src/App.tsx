import React from "react";
import { Routes, Route } from "react-router-dom";
import styled from "styled-components";
import SearchView from "./views/SearchView";
import MovieDetailView from "./views/MovieDetailView";
import Error404 from "./views/Error404";
import GlobalStyles from "./styles/global";
import Header from "./components/Header";

const AppContainer = styled.div`
  text-align: center;
`;

const App: React.FC = () => {
  return (
    <AppContainer>
      <GlobalStyles />
      <Header />
      <Routes>
        <Route path="/" element={<SearchView />} />
        <Route path="/movie/:id" element={<MovieDetailView />} />
        <Route path="*" element={<Error404 />} />
      </Routes>
    </AppContainer>
  );
};

export default App;
