import { fireEvent, render } from "@/tests";
import { Input } from "./Input";

describe("Input", () => {
  it("should render the label", () => {
    const { getByText } = render(<Input label="Label" value="" onChange={() => {}} />);

    expect(getByText("Label")).toBeInTheDocument();
  });

  it("should render the value", () => {
    const { getByDisplayValue } = render(<Input label="Label" value="Value" onChange={() => {}} />);

    expect(getByDisplayValue("Value")).toBeInTheDocument();
  });

  it("should call onChange when the input value changes", () => {
    const onChangeMock = vi.fn();
    const { getByDisplayValue } = render(
      <Input label="Label" value="Value" onChange={onChangeMock} />,
    );

    const inputElem = getByDisplayValue("Value");
    fireEvent.change(inputElem, { target: { value: "New value" } });

    expect(onChangeMock).toHaveBeenCalledWith("New value");
  });

  it("should render the clear button when there is a value in the input field", () => {
    const onChangeMock = vi.fn();
    const { queryByRole } = render(<Input label="Label" value="" onChange={onChangeMock} />);

    expect(queryByRole("button")).not.toBeInTheDocument();
  });

  it("should call onChange with an empty string when the clear button is clicked", () => {
    const onChangeMock = vi.fn();
    const { getByRole } = render(<Input label="Label" value="Value" onChange={onChangeMock} />);

    const clearButtonElem = getByRole("button");
    fireEvent.click(clearButtonElem);

    expect(onChangeMock).toHaveBeenCalledWith("");
  });
});
