import { render, screen } from "@testing-library/react";
import Poster from "./Poster";
import { ThemeProvider } from "styled-components";
import { theme } from "../../theme";

describe("Poster Component", () => {
	test("renders with a valid poster path", () => {
		render(
			<ThemeProvider theme={theme}>
				<Poster
					posterPath="https://via.placeholder.com/300x450"
					alt="Test Poster"
				/>
			</ThemeProvider>
		);
		const imgElement = screen.getByAltText("Test Poster");
		expect(imgElement).toBeInTheDocument();
		expect(imgElement).toHaveAttribute(
			"src",
			"https://via.placeholder.com/300x450"
		);
	});

	test("renders with a fallback image when posterPath is null", () => {
		render(
			<ThemeProvider theme={theme}>
				<Poster posterPath={null} alt="Fallback Poster" />
			</ThemeProvider>
		);
		const imgElement = screen.getByAltText("Fallback Poster");
		expect(imgElement).toBeInTheDocument();
		expect(imgElement).toHaveAttribute(
			"src",
			"https://via.placeholder.com/300x450?text=No+Image"
		);
	});

	test("applies lazy loading to the image", () => {
		render(
			<ThemeProvider theme={theme}>
				<Poster
					posterPath="https://via.placeholder.com/300x450"
					alt="Lazy Load Poster"
				/>
			</ThemeProvider>
		);
		const imgElement = screen.getByAltText("Lazy Load Poster");
		expect(imgElement).toHaveAttribute("loading", "lazy");
	});
});
