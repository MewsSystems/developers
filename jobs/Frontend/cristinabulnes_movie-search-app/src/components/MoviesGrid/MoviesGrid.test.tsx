import { render, screen, fireEvent } from "@testing-library/react";
import MoviesGrid from "./MoviesGrid";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import { mockMovies } from "../../__mocks__/mockMovies";
import { ThemeProvider } from "styled-components";
import { theme } from "../../theme";

// Mock navigate function (since we're using react-router-dom)
const mockNavigate = jest.fn();

jest.mock("react-router-dom", () => ({
	...jest.requireActual("react-router-dom"),
	useNavigate: () => mockNavigate,
}));

describe("MoviesGrid", () => {
	test("renders a list of MovieCard components", () => {
		render(
			<ThemeProvider theme={theme}>
				<MemoryRouter>
					<MoviesGrid movies={mockMovies} />
				</MemoryRouter>
			</ThemeProvider>
		);

		// Check that MovieCard components are rendered
		expect(
			screen.getByTestId(`base-movie-card-${mockMovies[0].id}`)
		).toBeInTheDocument();
		expect(
			screen.getByTestId(`base-movie-card-${mockMovies[1].id}`)
		).toBeInTheDocument();
	});

	test("triggers navigation when a MovieCard is clicked", () => {
		render(
			<ThemeProvider theme={theme}>
				<MemoryRouter initialEntries={["/"]}>
					<MoviesGrid movies={mockMovies} />
					<Routes>
						<Route path="/movie/:id" element={<div>Movie Details</div>} />
					</Routes>
				</MemoryRouter>
			</ThemeProvider>
		);

		// Simulate a click on the first movie
		const movieCard1 = screen.getByTestId(
			`base-movie-card-${mockMovies[0].id}`
		);
		fireEvent.click(movieCard1);

		// Check that navigate was called with the correct URL
		expect(mockNavigate).toHaveBeenCalledWith(`/movie/${mockMovies[0].id}`);
	});
});
