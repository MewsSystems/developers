import React from "react";
import * as ReactDOM from "react-dom/client";
import styled, { ThemeProvider, DefaultTheme } from "styled-components";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import SearchPage from "./pages/SearchPage";
import MovieDetailPage from "./pages/MovieDetailPage";
import Header from "./components/Header";
import Footer from "./components/Footer";
import GlobalStyle from "./globalStyles";

const theme: DefaultTheme = {
  colors: {
    primary: "mediumseagreen",
    secondary: "white",
    background: "#333",
    link: "#70757a",
  },
};

const GlobalContainer = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
`;

const router = createBrowserRouter([
  {
    path: "/",
    element: <SearchPage />,
  },
  {
    path: "/movie/:id",
    element: <MovieDetailPage />,
  },
]);

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <React.StrictMode>
    <ThemeProvider theme={theme}>
      <GlobalStyle />
      <GlobalContainer>
        <Header />
        <RouterProvider router={router} />
        <Footer />
      </GlobalContainer>
    </ThemeProvider>
  </React.StrictMode>
);
