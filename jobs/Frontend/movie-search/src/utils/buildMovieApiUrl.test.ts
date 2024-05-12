import { buildMovieApiUrl } from "./buildMovieApiUrl";

describe("buildMovieApiUrl", () => {
  // Save original environment variables
  const originalEnv = process.env;

  beforeEach(() => {
    // Clear all environment variables before each test
    jest.resetModules();
    process.env = { ...originalEnv };
  });

  afterAll(() => {
    // Restore original environment variables after all tests
    process.env = originalEnv;
  });

  it("builds the correct URL without a query", () => {
    process.env.MOVIE_DB_API_BASE_URL = "http://test-url/";
    process.env.MOVIE_DB_API_KEY = "test-key";

    const url = buildMovieApiUrl("test-endpoint");

    expect(url).toBe(
      "http://test-url/test-endpoint?api_key=test-key&include_adult=false&language=en-US&page=1"
    );
  });

  it("builds the correct URL with a query", () => {
    process.env.MOVIE_DB_API_BASE_URL = "http://test-url/";
    process.env.MOVIE_DB_API_KEY = "test-key";

    const url = buildMovieApiUrl("test-endpoint", "test-query");

    expect(url).toBe(
      "http://test-url/test-endpoint?api_key=test-key&include_adult=false&language=en-US&page=1&query=test-query"
    );
  });

  it("builds the correct URL with a query and a page number", () => {
    process.env.MOVIE_DB_API_BASE_URL = "http://test-url/";
    process.env.MOVIE_DB_API_KEY = "test-key";

    const url = buildMovieApiUrl("test-endpoint", "test-query", 2);

    expect(url).toBe(
      "http://test-url/test-endpoint?api_key=test-key&include_adult=false&language=en-US&page=2&query=test-query"
    );
  });
});
