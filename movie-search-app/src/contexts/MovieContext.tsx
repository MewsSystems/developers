// MovieContext.tsx
import { createContext, useContext } from "react";
import type { MovieContextType } from "../types/types";

// This file defines the MovieContext for managing movie data in the app.

// Context
export const MovieContext = createContext<MovieContextType | undefined>(
  undefined
);

export const useMovieContext = () => {
  const context = useContext(MovieContext);

  if (context === undefined) {
    throw new Error("useMovieContext must be used within a MovieProvider");
  }

  return context;
};
