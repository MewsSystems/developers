import { render, fireEvent } from "@/tests";
import { Button } from "./Button";

describe("Button", () => {
  it("should render correctly", () => {
    const { getByText } = render(<Button>Hello world!</Button>);

    expect(getByText("Hello world!")).toBeInTheDocument();
  });
  it("should call callback on click", () => {
    const onClick = vitest.fn();
    const { getByText } = render(<Button onClick={onClick}>Hello world!</Button>);

    const btnElem = getByText("Hello world!");
    fireEvent.click(btnElem);

    expect(onClick).toHaveBeenCalledTimes(1);
  });
  it("should pass basic HTML attributes", () => {
    const { getByText } = render(<Button type="submit">Hello world!</Button>);

    const btnElem = getByText("Hello world!");

    expect(btnElem).toHaveAttribute("type", "submit");
  });
});
