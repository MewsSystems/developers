import React from "react";
import { BrowserRouter, Route } from "react-router-dom";
import SearchView from "./components/SearchView";
import MovieDetailView from "./components/MovieDetailView";
import { createStore } from "redux";
import { Provider } from "react-redux";
import styled from "styled-components";
import "./components/style.css";

const initialState = {
  searchTerm: "",
};

export const reducer = (state = initialState, action: any) => {
  switch (action.type) {
    case "UPDATE_SEARCH_TERM":
      return { ...state, searchTerm: action.searchTerm };
    case "CLEAR_SEARCH_TERM":
      return { ...state, searchTerm: "" };
    default:
      return state;
  }
};

const store = createStore(reducer);

const StyledApp = styled.div`
  width: 100vw;
  min-height: 100vh;
  box-shadow: inset 0 20vh 33vw black;
  background: linear-gradient(
      217deg,
      rgba(200, 100, 0, 0.8),
      rgba(255, 0, 0, 0) 70.71%
    ),
    linear-gradient(127deg, rgba(0, 127, 63, 0.8), rgba(0, 31, 0, 0) 70.71%),
    radial-gradient(rgba(127, 127, 0, 0.4), rgba(0, 31, 0, 0) 90.71%),
    linear-gradient(336deg, rgba(0, 16, 62, 0.9), rgba(0, 0, 0, 0) 70.71%);
  font-family: sans-serif;
  color: white;
`;

const App: React.FC = () => {
  return (
    <Provider store={store}>
      <StyledApp>
        <BrowserRouter>
          <Route exact path="/">
            <SearchView />
          </Route>
          <Route path="/movie/:movieId">
            <MovieDetailView />
          </Route>
        </BrowserRouter>
      </StyledApp>
    </Provider>
  );
};
export default App;
