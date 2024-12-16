import { render, screen, fireEvent } from "@testing-library/react";
import Button from "./Button";

describe("Button", () => {
	it("renders the button with the correct label", async () => {
		render(<Button onClick={() => {}}>Click Me</Button>);
		expect(screen.getByRole("button")).toBeInTheDocument();
	});

	it("calls onClick when clicked", () => {
		const handleClick = jest.fn();
		render(<Button onClick={handleClick}>Click Me</Button>);
		fireEvent.click(screen.getByRole("button"));
		expect(handleClick).toHaveBeenCalledTimes(1);
	});
});
