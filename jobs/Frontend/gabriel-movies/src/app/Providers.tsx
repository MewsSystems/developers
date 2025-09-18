import { QueryClientProvider } from "@tanstack/react-query";
import { ThemeProvider } from "styled-components";
import { RouterProvider } from "react-router-dom";
import { queryClient } from "./config/queryClient";
import { theme } from "./styles/theme";
import { router } from "./router";

export function Providers() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <RouterProvider router={router} />
      </ThemeProvider>
    </QueryClientProvider>
  );
}
