import getPosterPath from "../../utils/ui";
import { customRender, screen } from "../../utils/testUtils";
import Poster from "./Poster";

describe("Poster Component", () => {
	test("renders with a valid poster path", () => {
		const posterPath = "/poster-path";
		customRender(<Poster posterPath={posterPath} alt="Test Poster" />);

		const imgElement = screen.getByAltText("Test Poster");

		const posterSrc = getPosterPath(posterPath);
		expect(imgElement).toHaveAttribute("src", posterSrc);
		expect(imgElement).toBeInTheDocument();
	});

	test("renders with a fallback image when posterPath is null", () => {
		const posterPath = null;
		customRender(<Poster posterPath={posterPath} alt="Fallback Poster" />);
		const imgElement = screen.getByAltText("Fallback Poster");
		expect(imgElement).toBeInTheDocument();
		const posterSrc = getPosterPath(posterPath);
		expect(imgElement).toHaveAttribute("src", posterSrc);
	});

	test("applies lazy loading to the image", () => {
		customRender(<Poster posterPath="/poster-path" alt="Lazy Load Poster" />);
		const imgElement = screen.getByAltText("Lazy Load Poster");
		expect(imgElement).toHaveAttribute("loading", "lazy");
	});
});
