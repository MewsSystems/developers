import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { customRender } from "../../utils/testUtils";
import MovieDetailsCard from "./MovieDetailsCard";

describe("MovieDetailsCard Component", () => {
	const mockMovie = {
		title: "Inception",
		posterPath: null,
		releaseDate: "2010-07-16",
		overview: "A mind-bending thriller.",
		genres: [{ name: "Action" }, { name: "Sci-Fi" }],
		rating: 8.8,
	};

	const mockOnGoBack = jest.fn();

	it("renders all movie details correctly", () => {
		customRender(
			<MovieDetailsCard
				title={mockMovie.title}
				posterPath={mockMovie.posterPath}
				releaseDate={mockMovie.releaseDate}
				overview={mockMovie.overview}
				genres={mockMovie.genres}
				rating={mockMovie.rating}
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
		expect(screen.getByText("Genres: Action, Sci-Fi")).toBeInTheDocument();

		// Validate overview
		expect(screen.getByText(mockMovie.overview)).toBeInTheDocument();

		// Validate rating
		expect(
			screen.getByText(`Rating: ${mockMovie.rating}/10`)
		).toBeInTheDocument();

		// Validate poster image
		const posterSrc =
			mockMovie.posterPath ||
			"https://via.placeholder.com/300x450?text=No+Image";
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
				rating={mockMovie.rating}
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
