import { MovieDetail } from "../models/movies.types";
import { getMovieDetailEndpoint } from "./movies-service";


//need fixing (library used to call the api is giving some mocking problems , probably i should used axios)
describe("service to call movies API", () => {
  // Applies only to tests in this describe block
  beforeEach(() => {});

  describe("getMovieDetailEndpoint", () => {
    it("should call wretch library and obtain reponse", async () => {
      const result = await getMovieDetailEndpoint(1);
      const movieDetails: MovieDetail = {
        adult: false,
        budget: 10000,
        revenue: 10000,
        overview: "test",
        releaseDate: "2023-07-31",
        posterPath: "/test.jpg",
        popularity: 100,
        runtime: 112,
        status: "released",
        voteAverage: 10.31,
        voteCount: 1220,
        genres: "Comedy,Romance,Drama",
        id: 1,
        originalLanguage: "ES",
        originalTitle: "testOG",
        tagline: "testTag",
        title: "test title",
      };
      console.log(result);

      expect(result).toBe(movieDetails);
    });
  });
});
