import { customRender, screen, fireEvent } from "../../utils/testUtils";
import Input from "./Input";

const inputProps = {
	id: "test-input",
	name: "test-input",
	label: "Test Label",
	placeholder: "Type something",
	value: "",
	onChange: () => {},
};

describe("Input Component", () => {
	it("renders input with a label and not placeholder", () => {
		customRender(<Input {...inputProps} />);

		// Assert label is rendered
		const label = screen.getByText(inputProps.label);
		expect(label).toBeInTheDocument();

		// Assert input is rendered with no placeholder
		const input = screen.getByLabelText(inputProps.label);
		expect(input).toBeInTheDocument();
		expect(input).not.toHaveAttribute("placeholder", "Type something");
	});

	it("renders input with a placeholder", () => {
		customRender(<Input {...inputProps} label="" />);

		const input = screen.getByPlaceholderText(inputProps.placeholder);
		expect(input).toBeInTheDocument();
		expect(input).toHaveAttribute("placeholder", inputProps.placeholder);
	});

	it("calls onChange when value changes", () => {
		const handleChange = jest.fn();
		customRender(<Input {...inputProps} onChange={handleChange} />);

		const input = screen.getByLabelText(inputProps.label);
		fireEvent.change(input, { target: { value: "New Value" } });

		expect(handleChange).toHaveBeenCalledTimes(1);
	});

	it("renders disabled input", () => {
		customRender(<Input {...inputProps} disabled />);

		const input = screen.getByLabelText(inputProps.label);
		expect(input).toBeDisabled();
	});
});
