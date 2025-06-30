import { vi } from "vitest";

export const createPopularMoviesQueryMock = () => ({
  data: {
    pages: [
      {
        page: 1,
        total_pages: 2,
        total_results: 2,
        results: [
          {
            id: 1,
            title: "Test Movie 1",
            adult: false,
            backdrop_path: null,
            genre_ids: [],
            original_language: "en",
            original_title: "Test Movie 1",
            overview: "Test overview",
            poster_path: null,
            popularity: 100,
            release_date: "2023-01-01",
            video: false,
            vote_average: 7.5,
            vote_count: 1000,
          },
        ],
      },
    ],
  },
  fetchNextPage: vi.fn(),
  hasNextPage: true,
  isFetching: false,
  isFetchingNextPage: false,
  status: "success" as const,
});

export const createSearchMoviesQueryMock = () => ({
  data: {
    pages: [
      {
        page: 1,
        total_pages: 1,
        total_results: 1,
        results: [
          {
            id: 2,
            title: "Search Result",
            adult: false,
            backdrop_path: null,
            genre_ids: [],
            original_language: "en",
            original_title: "Search Result",
            overview: "Search overview",
            poster_path: null,
            popularity: 50,
            release_date: "2023-01-01",
            video: false,
            vote_average: 6.5,
            vote_count: 500,
          },
        ],
      },
    ],
  },
  fetchNextPage: vi.fn(),
  hasNextPage: false,
  isFetching: false,
  isFetchingNextPage: false,
  status: "success" as const,
});
