import { render, screen } from "@testing-library/react";
import { ThemeProvider } from "styled-components";
import BaseMovieCard from "./BaseMovieCard";
import { theme } from "../../theme";

describe("MovieCard Component", () => {
	const mockMovie = {
		id: "1",
		title: "Inception",
		posterPath: "/poster.jpg",
		releaseDate: "2010-07-16",
		rating: 8.8,
	};

	it("renders the MovieCard with correct data", () => {
		render(
			<ThemeProvider theme={theme}>
				<BaseMovieCard
					id={mockMovie.id}
					title={mockMovie.title}
					posterPath={mockMovie.posterPath}
					releaseDate={mockMovie.releaseDate}
					rating={mockMovie.rating}
				/>
			</ThemeProvider>
		);

		expect(screen.getByText(mockMovie.title)).toBeInTheDocument();
		expect(
			screen.getByText(`Release Date: ${mockMovie.releaseDate}`)
		).toBeInTheDocument();
		expect(
			screen.getByText(`Rating: ${mockMovie.rating}/10`)
		).toBeInTheDocument();
		expect(screen.getByAltText(`${mockMovie.title} Poster`)).toHaveAttribute(
			"src",
			`https://image.tmdb.org/t/p/w300${mockMovie.posterPath}`
		);
	});
});
