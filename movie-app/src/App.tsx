import { BrowserRouter, Route, Routes } from "react-router-dom";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import { CssBaseline, ThemeProvider } from "@mui/material";

import { ErrorBoundary } from "../ErrorBoundary.tsx";
import theme from "./styles/theme.tsx";

import "./App.css";
import HomePage from "@/pages/HomePage.tsx";
import MovieDetailPage from "@/pages/MovieDetailPage.tsx";
import NotFoundPage from "@/pages/NotFoundPage.tsx";

const queryClient = new QueryClient();

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <ErrorBoundary>
        <QueryClientProvider client={queryClient}>
          <BrowserRouter>
            <Routes>
              <Route path="/" element={<HomePage />} />
              <Route path="/movie/:id" element={<MovieDetailPage />} />
              <Route path="/*" element={<NotFoundPage />} />
            </Routes>
          </BrowserRouter>
        </QueryClientProvider>
      </ErrorBoundary>
    </ThemeProvider>
  );
}

export default App;
