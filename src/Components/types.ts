export interface Movie {
  id: number;
  title: string;
  backdrop_path: string;
}

export interface ApiResponse {
  results: Movie[];
  total_results: number;
  total_pages: number;
}
