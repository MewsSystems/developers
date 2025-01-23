/// <reference types="vite/client" />

interface SearchResult {
  original_title: string;
  original_language: string;
  release_date: Date;
  vote_average: number;
  vote_count: number;
  overview: string;
  popularity?: string;
  backdrop_path?: string;
  poster_path?: string;
}

interface TileProps {
  item: SearchResult
  openDetail: (movie: TileProps) => void;
}
