import { screen, render } from "@testing-library/react";

import * as useMovieDetail from "../useMovieDetail";
import { MoviesDetailView } from "../MoviesDetailView";
import { MovieDetailResponse } from "../types/movies";

const useMovieDetailSpy = vi.spyOn(useMovieDetail, "useMovieDetail");

describe("Movie Detail", () => {
  afterAll(() => {
    vi.clearAllMocks();
  });
  function Arrange() {
    render(<MoviesDetailView />);
  }

  const movieDetail: MovieDetailResponse = {
    backdrop_path: "/1XDDXPXGiI8id7MrUxK36ke7gkX.jpg",
    budget: 85000000,
    genres: [
      {
        id: 16,
        name: "Animation",
      },
      {
        id: 28,
        name: "Action",
      },
      {
        id: 10751,
        name: "Family",
      },
      {
        id: 35,
        name: "Comedy",
      },
      {
        id: 14,
        name: "Fantasy",
      },
    ],
    id: 1011985,
    original_title: "Kung Fu Panda 4",
    overview:
      "Po is gearing up to become the spiritual leader of his Valley of Peace, but also needs someone to take his place as Dragon Warrior. As such, he will train a new kung fu practitioner for the spot and will encounter a villain called the Chameleon who conjures villains from the past.",
    popularity: 2015.484,
    poster_path: "/kDp1vUBnMpe8ak4rjgl3cLELqjU.jpg",
    release_date: "2024-03-02",
    revenue: 452991725,
    status: "Released",
    title: "Kung Fu Panda 4",
    vote_average: 7.152,
    vote_count: 1112,
  };

  it("should render movie detail page", async () => {
    useMovieDetailSpy.mockReturnValue({ movieDetail: movieDetail });
    Arrange();

    const title = await screen.findByRole("heading", {
      name: "Kung Fu Panda 4",
    });
    expect(title).toBeInTheDocument(); // Use the toBeInTheDocument function
  });
});
