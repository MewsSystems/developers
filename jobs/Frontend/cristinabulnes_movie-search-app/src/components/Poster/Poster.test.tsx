import { customRender, screen } from "../../utils/testUtils";
import Poster from "./Poster";

describe("Poster Component", () => {
	test("renders with a valid poster path", () => {
		customRender(
			<Poster
				posterPath="https://via.placeholder.com/300x450"
				alt="Test Poster"
			/>
		);
		const imgElement = screen.getByAltText("Test Poster");
		expect(imgElement).toBeInTheDocument();
		expect(imgElement).toHaveAttribute(
			"src",
			"https://via.placeholder.com/300x450"
		);
	});

	test("renders with a fallback image when posterPath is null", () => {
		customRender(<Poster posterPath={null} alt="Fallback Poster" />);
		const imgElement = screen.getByAltText("Fallback Poster");
		expect(imgElement).toBeInTheDocument();
		expect(imgElement).toHaveAttribute(
			"src",
			"https://via.placeholder.com/300x450?text=No+Image"
		);
	});

	test("applies lazy loading to the image", () => {
		customRender(
			<Poster
				posterPath="https://via.placeholder.com/300x450"
				alt="Lazy Load Poster"
			/>
		);
		const imgElement = screen.getByAltText("Lazy Load Poster");
		expect(imgElement).toHaveAttribute("loading", "lazy");
	});
});
