import "./App.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import HomePage from "./pages/HomePage.tsx";
import MovieDetailPage from "./pages/MovieDetailPage.tsx";
import NotFoundPage from "./pages/NotFoundPage.tsx";
import { ErrorBoundary } from "../ErrorBoundary.tsx";
import theme from "./theme.tsx";
import { CssBaseline, ThemeProvider } from "@mui/material";

const queryClient = new QueryClient();

function App() {
  return (
    <>
      <ErrorBoundary>
        <ThemeProvider theme={theme}>
          <CssBaseline />
          <QueryClientProvider client={queryClient}>
            <BrowserRouter>
              <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/movie/:id" element={<MovieDetailPage />} />
                <Route path="/*" element={<NotFoundPage />} />
              </Routes>
            </BrowserRouter>
          </QueryClientProvider>
        </ThemeProvider>
      </ErrorBoundary>
    </>
  );
}

export default App;
