import * as api from "./movies"

describe("Movies api test", () => {
  it("Should call get from moviesApi instance", () => {
    const getSpy = vi.spyOn(api.moviesApi, "get")
    getSpy.mockResolvedValue("")

    api.getDicoverMovies()
    api.getMovieDetails("")
    api.getMovieSearch("")

    expect(getSpy).toBeCalledTimes(3)
  })
})
