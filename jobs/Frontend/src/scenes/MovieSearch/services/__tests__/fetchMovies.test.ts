import fetchMovies from "@/scenes/MovieSearch/services/fetchMovies";

const fetchSpy = jest.fn().mockImplementation(() =>
  Promise.resolve({
    json: () => Promise.resolve({}),
  } as Response),
);
global.fetch = fetchSpy;
const originalEnv = process.env;
describe("fetchMovies", () => {
  afterEach(() => {
    process.env = { ...originalEnv };
  });
  it("should call fetch", () => {
    process.env.NEXT_PUBLIC_MOVIES_API_KEY = "test-key";
    fetchMovies("test", 1);

    expect(fetchSpy).toHaveBeenCalledWith(
      "https://api.themoviedb.org/3/search/movie?api_key=test-key&query=test&page=1",
    );
  });
});
