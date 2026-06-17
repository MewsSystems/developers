import { render, fireEvent } from "@/tests";
import { IconButton } from "./IconButton";

describe("IconButton", () => {
  it("should render correctly", () => {
    const { getByRole, getByText } = render(
      <IconButton>
        <span>button child</span>
      </IconButton>,
    );

    expect(getByRole("button")).toBeInTheDocument();
    expect(getByText("button child")).toBeInTheDocument();
  });

  it("should call callback on click", () => {
    const onClick = vi.fn();
    const { getByRole } = render(<IconButton onClick={onClick} />);

    const btnElem = getByRole("button");
    fireEvent.click(btnElem);

    expect(onClick).toHaveBeenCalledTimes(1);
  });

  it("should pass basic HTML attributes", () => {
    const { getByRole } = render(<IconButton type="submit" />);

    const btnElem = getByRole("button");

    expect(btnElem).toHaveAttribute("type", "submit");
  });

  it("should apply styles based on props", () => {
    const { getByRole } = render(
      <>
        <IconButton size="large">Large primary</IconButton>
        <IconButton size="small">Small transparent</IconButton>
      </>,
    );

    const largeBtnElem = getByRole("button", { name: "Large primary" });
    const smallBtnElem = getByRole("button", { name: "Small transparent" });

    expect(largeBtnElem).toHaveStyle("padding: 16px");
    expect(smallBtnElem).toHaveStyle("padding: 4px");
  });

  it("should be disabled when disabled prop is passed", () => {
    const { getByRole } = render(<IconButton disabled />);

    const btnElem = getByRole("button");

    expect(btnElem).toBeDisabled();
  });
});
