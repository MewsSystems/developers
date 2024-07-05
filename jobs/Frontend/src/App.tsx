import React from "react";
import * as ReactDOM from "react-dom/client";
import styled, { ThemeProvider } from "styled-components";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { SearchContextProvider } from "./contexts/SearchContext";
import SearchPage from "./pages/SearchPage";
import MovieDetailPage from "./pages/MovieDetailPage";
import Header from "./components/Header";
import Footer from "./components/Footer";
import GlobalStyle from "./globalStyles";
import theme from "./theme";

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

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      gcTime: 10 * 60 * 1000, // keep the cache for 5 minutes
      // Retry failed requests 3 times
      // Except in the cypress tests where we want to fail fast
      // @ts-ignore
      retry: typeof Cypress !== "undefined" ? 0 : 3,
    },
  },
});

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement,
);
root.render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <SearchContextProvider>
          <GlobalStyle />
          <GlobalContainer>
            <Header />
            <RouterProvider router={router} />
            <Footer />
          </GlobalContainer>
        </SearchContextProvider>
      </ThemeProvider>
    </QueryClientProvider>
  </React.StrictMode>,
);
