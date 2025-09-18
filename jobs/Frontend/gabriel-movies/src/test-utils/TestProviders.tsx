import type { ReactNode } from "react";
import { QueryClientProvider } from "@tanstack/react-query";
import { ThemeProvider } from "styled-components";
import { MemoryRouter } from "react-router-dom";
import { theme } from "@/app/styles/theme";
import { queryClient } from "@/app/config/queryClient";

export type TestProvidersProps = {
  children: ReactNode;
  initialEntries?: string[];
};

export function TestProviders({
  children,
  initialEntries = ["/"],
}: TestProvidersProps) {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <MemoryRouter initialEntries={initialEntries}>{children}</MemoryRouter>
      </ThemeProvider>
    </QueryClientProvider>
  );
}
