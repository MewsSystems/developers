import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { customRender } from "../../utils/testUtils";
import MovieDetailsCard from "./MovieDetailsCard";
import { mockMovie } from "../../__mocks__/mockMovies";
import arrayToString from "../../utils/general";
import getPosterPath from "../../utils/ui";

describe("MovieDetailsCard Component", () => {
	const mockOnGoBack = jest.fn();

	it("renders all movie details correctly", () => {
		customRender(
			<MovieDetailsCard
				title={mockMovie.title}
				posterPath={mockMovie.posterPath}
				releaseDate={mockMovie.releaseDate}
				overview={mockMovie.overview}
				genres={mockMovie.genres}
				rating={mockMovie.voteAverage}
				onGoBack={mockOnGoBack}
			/>
		);

		// Validate title
		expect(screen.getByText(mockMovie.title)).toBeInTheDocument();

		// Validate release date
		expect(
			screen.getByText(`Release Date: ${mockMovie.releaseDate}`)
		).toBeInTheDocument();
		// Validate genres
		const formattedGenres = arrayToString(mockMovie.genres, "name");
		expect(screen.getByText(`Genres: ${formattedGenres}`)).toBeInTheDocument();

		// Validate overview
		expect(screen.getByText(mockMovie.overview)).toBeInTheDocument();

		// Validate rating
		expect(
			screen.getByText(`Rating: ${mockMovie.voteAverage}/10`)
		).toBeInTheDocument();

		// Validate poster image
		const posterSrc = getPosterPath(mockMovie.posterPath);
		const posterImage = screen.getByAltText(`${mockMovie.title} Poster`);
		expect(posterImage).toHaveAttribute("src", posterSrc);
	});

	it("renders default poster image when posterPath is null", () => {
		customRender(
			<MovieDetailsCard
				title={mockMovie.title}
				posterPath={null}
				releaseDate={mockMovie.releaseDate}
				overview={mockMovie.overview}
				genres={mockMovie.genres}
				rating={mockMovie.voteAverage}
				onGoBack={mockOnGoBack}
			/>
		);

		// Validate default poster image
		const posterSrc = getPosterPath(null);
		const posterImage = screen.getByAltText(`${mockMovie.title} Poster`);
		expect(posterImage).toHaveAttribute("src", posterSrc);
	});

	it("calls the onGoBack function when the Go back! button is clicked", async () => {
		customRender(
			<MovieDetailsCard
				title={mockMovie.title}
				posterPath={mockMovie.posterPath}
				releaseDate={mockMovie.releaseDate}
				overview={mockMovie.overview}
				genres={mockMovie.genres}
				rating={mockMovie.voteAverage}
				onGoBack={mockOnGoBack}
			/>
		);

		// Simulate button click
		const goBackButton = screen.getByRole("button", { name: /go back!/i });
		await userEvent.click(goBackButton);

		// Validate callback
		expect(mockOnGoBack).toHaveBeenCalledTimes(1);
	});
});
