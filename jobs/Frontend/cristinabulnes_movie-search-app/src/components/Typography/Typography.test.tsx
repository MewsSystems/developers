import { theme } from "../../theme";
import { customRender, screen } from "../../utils/testUtils";
import Typography from "./Typography";

describe("Typography Component", () => {
	it("renders the correct text content", () => {
		customRender(<Typography>Test Text</Typography>);
		expect(screen.getByText("Test Text")).toBeInTheDocument();
	});

	it("applies the correct variant styles", () => {
		customRender(<Typography variant="h1">Header 1</Typography>);
		const element = screen.getByText("Header 1");
		expect(element.tagName).toBe("P");
		expect(element).toHaveStyle(`font-size: ${theme.typography.h1.fontSize}`);
	});

	it("renders with a custom element using the `as` prop", () => {
		customRender(
			<Typography as="h2" variant="h2">
				Header 2
			</Typography>
		);
		const element = screen.getByText("Header 2");
		expect(element.tagName).toBe("H2");
	});

	it("applies custom color, fontWeight, and fontSize", () => {
		customRender(
			<Typography color="red" fontWeight="700" fontSize="20px">
				Custom Text
			</Typography>
		);
		const element = screen.getByText("Custom Text");
		expect(element).toHaveStyle("color: red");
		expect(element).toHaveStyle("font-weight: 700");
		expect(element).toHaveStyle("font-size: 20px");
	});

	it("defaults to body variant when no variant is provided", () => {
		customRender(<Typography>Default Text</Typography>);
		const element = screen.getByText("Default Text");
		expect(element).toHaveStyle(
			`font-size: ${theme.typography.body2.fontSize}`
		);
	});
});
