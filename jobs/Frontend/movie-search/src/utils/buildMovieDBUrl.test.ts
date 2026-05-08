// Update with the correct file name

import { buildMovieDBUrl } from "./buildMovieDBUrl";

describe("buildMovieDBUrl function", () => {
  // Mock environment variables
  const OLD_ENV = process.env;

  beforeEach(() => {
    jest.resetModules(); // Clear the module cache before each test
    process.env = { ...OLD_ENV }; // Restore mock environment variables
  });

  afterAll(() => {
    process.env = OLD_ENV; // Restore original environment variables after all tests
  });

  it("should build a MovieDB URL without query", () => {
    process.env.NEXT_PUBLIC_MOVIE_DB_API_BASE_URL =
      "https://api.themoviedb.org/3";
    process.env.NEXT_PUBLIC_MOVIE_DB_API_KEY = "test_api_key";

    const expectedUrl =
      "https://api.themoviedb.org/3/movies?api_key=test_api_key&include_adult=false&language=en-US&page=1";
    expect(buildMovieDBUrl("movies")).toEqual(expectedUrl);
  });

  it("should build a MovieDB URL with query", () => {
    process.env.NEXT_PUBLIC_MOVIE_DB_API_BASE_URL =
      "https://api.themoviedb.org/3";
    process.env.NEXT_PUBLIC_MOVIE_DB_API_KEY = "test_api_key";

    const expectedUrl =
      "https://api.themoviedb.org/3/search?api_key=test_api_key&include_adult=false&language=en-US&page=2&query=star+wars";
    expect(buildMovieDBUrl("search", "star wars", 2)).toEqual(expectedUrl);
  });

  it("should build a MovieDB URL with default page", () => {
    process.env.NEXT_PUBLIC_MOVIE_DB_API_BASE_URL =
      "https://api.themoviedb.org/3";
    process.env.NEXT_PUBLIC_MOVIE_DB_API_KEY = "test_api_key";

    const expectedUrl =
      "https://api.themoviedb.org/3/movies?api_key=test_api_key&include_adult=false&language=en-US&page=1";
    expect(buildMovieDBUrl("movies")).toEqual(expectedUrl);
  });
});
