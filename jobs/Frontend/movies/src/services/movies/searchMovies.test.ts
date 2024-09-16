import { searchResponseMock } from "./movies.factory";
import { searchMovies, searchMoviesQueryOptions } from "./searchMovies";

describe("searchMovies", () => {
  it("should return normalized data", async () => {
    vi.stubGlobal(
      "fetch",
      vi.fn().mockResolvedValue({
        ok: true,
        json: () => Promise.resolve(searchResponseMock.build()),
      }),
    );

    const result = await searchMovies({ query: "harry", page: 1 });

    expect(result).toMatchObject({
      page: 1,
      totalPages: 1,
      totalResults: 1,
      movies: [
        {
          id: 672,
          overview:
            "Cars fly, trees fight back, and a mysterious house-elf comes to warn Harry Potter at the start of his second year at Hogwarts. Adventure and danger await when bloody writing on a wall announces: The Chamber Of Secrets Has Been Opened. To save Hogwarts will require all of Harry, Ron and Hermione’s magical abilities and courage.",
          title: "Harry Potter and the Chamber of Secrets",
          rating: 7.7,
          imgSrc:
            "https://image.tmdb.org/t/p/w500/sdEOH0992YZ0QSxgXNIGLq1ToUi.jpg",
        },
      ],
    });
  });
});

describe("searchMoviesQueryOptions", () => {
  it("should return query options", () => {
    const result = searchMoviesQueryOptions({ page: 1, query: "harry" });

    expect(result).toMatchObject({
      queryKey: ["movies", { page: 1, query: "harry" }],
    });
  });
});
