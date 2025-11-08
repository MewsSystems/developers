import { Movie } from "@/scenes/MovieSearch/services/types";

export const testMovie: Movie = {
  id: 1,
  title: "Test Movie",
  release_date: "2021-01-01",
  poster_path: "test.jpg",
  backdrop_path: null,
  overview: "Test overview",
  vote_average: 5,
  vote_count: 100,
  adult: false,
  original_language: "en",
  original_title: "Test Movie",
  popularity: 100,
  video: false,
  genre_ids: [1, 2],
};
