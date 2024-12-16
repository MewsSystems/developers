import { render, screen, fireEvent } from "@testing-library/react";
import MovieCard from "./MovieCard";
import { ThemeProvider } from "styled-components";
import { theme } from "../../theme";

describe("MovieCard", () => {
	const movieProps = {
		id: "1",
		title: "Test Movie",
		posterPath: null,
		releaseDate: "2024-01-01",
		rating: 8,
		onClick: jest.fn(),
	};

	test("renders correctly with given props", () => {
		render(
			<ThemeProvider theme={theme}>
				<MovieCard {...movieProps} />
			</ThemeProvider>
		);

		// Check if the title, poster, release date, and rating are rendered
		expect(screen.getByText("Test Movie")).toBeInTheDocument();
		expect(screen.getByAltText(`${movieProps.title} Poster`)).toHaveAttribute(
			"src",
			"https://via.placeholder.com/300x450?text=No+Image"
		);
		expect(
			screen.getByText(`Release Date: ${movieProps.releaseDate}`)
		).toBeInTheDocument();
		expect(
			screen.getByText(`Rating: ${movieProps.rating}/10`)
		).toBeInTheDocument();
	});

	test("calls onClick when the card is clicked", () => {
		render(
			<ThemeProvider theme={theme}>
				<MovieCard {...movieProps} />
			</ThemeProvider>
		);

		const cardContainer = screen.getByRole("button");

		// Simulate a click event
		fireEvent.click(cardContainer);

		// Check that the onClick function was called
		expect(movieProps.onClick).toHaveBeenCalledTimes(1);
	});

	test("applies hover effect when hovered", async () => {
		render(
			<ThemeProvider theme={theme}>
				<MovieCard {...movieProps} />
			</ThemeProvider>
		);

		const cardContainer = screen.getByRole("button");

		// Simulate hover event
		fireEvent.mouseOver(cardContainer);

		// Assert that the transform style is applied (check for the hover effect)
		expect(cardContainer).toHaveStyle("transform: scale(1.02)");
	});

	test("applies focus effect when focused", () => {
		render(
			<ThemeProvider theme={theme}>
				<MovieCard {...movieProps} />
			</ThemeProvider>
		);

		const cardContainer = screen.getByRole("button");

		// Simulate focus event
		fireEvent.focus(cardContainer);

		// Check that the focus style is applied
		expect(cardContainer).toHaveStyle(`box-shadow: ${theme.shadows.hover}`);
	});
});
