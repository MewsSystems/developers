import { customRender, screen } from "../../utils/testUtils";
import BaseCard from "./BaseCard";

describe("BaseCard Component", () => {
	const mockData = {
		footer: "Footer",
		title: "Title",
		info: "Info",
		posterPath: "https://example.com/poster.jpg",
	};

	it("renders the BaseCard with default layout and size", () => {
		customRender(
			<BaseCard size={3}>
				<BaseCard.Body $layout="row">
					<BaseCard.Poster
						posterPath={mockData.posterPath}
						alt={`${mockData.title} Poster`}
					/>
					<BaseCard.Content>
						<h2>{mockData.title}</h2>
						<p>{mockData.info}</p>
					</BaseCard.Content>
				</BaseCard.Body>
				<BaseCard.Footer>
					<p>{mockData.footer}</p>
				</BaseCard.Footer>
			</BaseCard>
		);

		// Validate content
		expect(screen.getByText(mockData.title)).toBeInTheDocument();

		// Validate poster image
		expect(screen.getByAltText(`${mockData.title} Poster`)).toHaveAttribute(
			"src",
			mockData.posterPath
		);

		// Validate layout (Body should have row layout)
		const body = screen.getByAltText(`${mockData.title} Poster`).parentElement;
		expect(body).toHaveStyle("flex-direction: row");

		// Ensure proper size-related styles are applied (padding and gap)
		const container = screen.getByTestId("card-container");
		expect(container).toHaveStyle(
			"padding: calc(var(--spacing-base) * var(--spacing-multiplier))"
		);
		expect(container).toHaveStyle(
			"gap: calc(var(--spacing-base) * var(--spacing-multiplier) * 0.5)"
		);
	});
});
