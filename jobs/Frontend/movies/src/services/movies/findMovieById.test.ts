import { fetchMovieById, findMovieByIdQueryOptions } from "./findMovieById";
import { movieResponseMock } from "./movies.factory";

describe("fetchMovieById", () => {
  it("should return the normalized data", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue({
        ok: true,
        json: () => Promise.resolve(movieResponseMock.build()),
      }),
    );
    const result = await fetchMovieById("1");

    expect(result).toMatchObject({
      id: 672,
      title: "Harry Potter and the Chamber of Secrets",
      overview:
        "Cars fly, trees fight back, and a mysterious house-elf comes to warn Harry Potter at the start of his second year at Hogwarts. Adventure and danger await when bloody writing on a wall announces: The Chamber Of Secrets Has Been Opened. To save Hogwarts will require all of Harry, Ron and Hermione’s magical abilities and courage.",
      runtime: 161,
      rating: 7.7,
      imgSrc:
        "https://image.tmdb.org/t/p/original//1stUIsjawROZxjiCMtqqXqgfZWC.jpg",
      genres: ["Adventure", "Fantasy"],
    });
  });
});

describe("findMovieByIdQueryOptions", () => {
  it("should return query options", () => {
    const result = findMovieByIdQueryOptions("1");

    expect(result).toMatchObject({
      queryKey: ["movies", "1"],
    });
  });
});
