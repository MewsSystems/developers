import { customRender, screen, fireEvent } from "../../utils/testUtils";
import Button from "./Button";

describe("Button", () => {
	it("renders the button with the correct label", async () => {
		customRender(<Button onClick={() => {}}>Click Me</Button>);
		expect(screen.getByRole("button")).toBeInTheDocument();
	});

	test("disables Button when disabled prop is passed", () => {
		customRender(
			<Button onClick={() => {}} disabled>
				Click me
			</Button>
		);
		const button = screen.getByRole("button");
		expect(button).toBeDisabled();
	});

	it("calls onClick when clicked", () => {
		const handleClick = jest.fn();
		customRender(<Button onClick={handleClick}>Click Me</Button>);
		fireEvent.click(screen.getByRole("button"));
		expect(handleClick).toHaveBeenCalledTimes(1);
	});
});
