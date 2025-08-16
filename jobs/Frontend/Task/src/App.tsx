import React from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { MovieSearch } from "./pages/MovieSearch/MovieSearch";
import { THEME_ID, ThemeProvider } from "@mui/material";
import { theme } from "./utils/theme";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
    },
  },
});

export const App = () => {
  return (
    <ThemeProvider theme={{ [THEME_ID]: theme }}>
      <QueryClientProvider client={queryClient}>
        <MovieSearch />
      </QueryClientProvider>
    </ThemeProvider>
  );
};
