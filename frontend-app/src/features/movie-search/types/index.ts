/**
 * Movie search feature types
 */
export interface SearchState {
  query: string;
  isLoading: boolean;
  error: string | null;
}

export interface SearchActions {
  setQuery: (query: string) => void;
  clearSearch: () => void;
}