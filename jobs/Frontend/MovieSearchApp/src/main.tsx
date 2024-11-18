import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { MovieSearch } from "./MovieSearch/MovieSearch";
import "./index.css";

const queryClient = new QueryClient();

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <main className="flex flex-col min-h-screen p-4 min-w-[500px] max-w-5xl m-auto">
        <MovieSearch />
      </main>
    </QueryClientProvider>
  </StrictMode>
);
