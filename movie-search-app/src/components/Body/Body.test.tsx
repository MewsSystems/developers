import { render, screen } from "@testing-library/react";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { MovieProvider } from "@contexts/MovieProvider";
import { Body } from "./Body";
import { vi } from "vitest";

vi.mock("@api/getPopularMovies", () => ({
  getPopularMovies: () =>
    Promise.resolve({
      results: [
        {
          id: 1,
          title: "Test Title",
          overview: "Test Overview",
          release_date: "2022-10-16",
          poster_path: "",
          vote_average: 4.0,
          popularity: 50,
          runtime: 120,
        },
      ],
      total_pages: 1,
      total_results: 1,
      page: 1,
    }),
}));

test("Body shows mocked movie title", async () => {
  render(
    <MovieProvider>
      <MemoryRouter initialEntries={["/movies/1"]}>
        <Routes>
          <Route path="/movies/:page" element={<Body />} />
        </Routes>
      </MemoryRouter>
    </MovieProvider>
  );

  expect(await screen.findByText(/Test Title/i)).toBeInTheDocument();
});
