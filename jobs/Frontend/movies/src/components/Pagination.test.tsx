import { fireEvent, render } from "@testing-library/react";
import { Pagination } from "./Pagination";

describe("Pagination", () => {
  it("should render Pagination", () => {
    const screen = render(
      <Pagination currentPage={1} totalPages={3} onPageChange={vi.fn} />,
    );

    expect(screen.getByText("1")).toBeInTheDocument();
    expect(screen.getByText("2")).toBeInTheDocument();
    expect(screen.getByText("3")).toBeInTheDocument();
    expect(screen.getByTestId("greater")).toBeInTheDocument();
  });

  it("should render < numbers, dots and >", () => {
    const screen = render(
      <Pagination currentPage={7} totalPages={30} onPageChange={vi.fn} />,
    );

    expect(screen.getByTestId("lower")).toBeInTheDocument();
    expect(screen.getByText("1")).toBeInTheDocument();
    expect(screen.getByTestId("dots-1")).toBeInTheDocument();
    expect(screen.getByText("6")).toBeInTheDocument();
    expect(screen.getByText("7")).toBeInTheDocument();
    expect(screen.getByText("8")).toBeInTheDocument();
    expect(screen.getByTestId("dots-5")).toBeInTheDocument();
    expect(screen.getByText("30")).toBeInTheDocument();
    expect(screen.getByTestId("greater")).toBeInTheDocument();
  });

  it("should render nothing when there is not more than one page", () => {
    const screen = render(
      <Pagination currentPage={1} totalPages={1} onPageChange={vi.fn} />,
    );

    expect(screen.container.firstChild).toBeNull();
  });

  it("should show only right dots", () => {
    const screen = render(
      <Pagination currentPage={1} totalPages={7} onPageChange={vi.fn} />,
    );

    expect(screen.getByText("1")).toBeInTheDocument();
    expect(screen.getByText("2")).toBeInTheDocument();
    expect(screen.getByText("3")).toBeInTheDocument();
    expect(screen.getByText("4")).toBeInTheDocument();
    expect(screen.getByText("5")).toBeInTheDocument();
    expect(screen.getByTestId("dots-5")).toBeInTheDocument();
    expect(screen.getByText("7")).toBeInTheDocument();
    expect(screen.getByTestId("greater")).toBeInTheDocument();
  });

  it("should show only right dots", () => {
    const screen = render(
      <Pagination currentPage={30} totalPages={30} onPageChange={vi.fn} />,
    );

    expect(screen.getByTestId("lower")).toBeInTheDocument();
    expect(screen.getByText("26")).toBeInTheDocument();
    expect(screen.getByText("27")).toBeInTheDocument();
    expect(screen.getByText("28")).toBeInTheDocument();
    expect(screen.getByText("29")).toBeInTheDocument();
    expect(screen.getByText("30")).toBeInTheDocument();
  });

  it("should test onPageChange cases", () => {
    const onPageChangeMock = vi.fn();
    const screen = render(
      <Pagination
        currentPage={7}
        totalPages={30}
        onPageChange={onPageChangeMock}
      />,
    );

    fireEvent.click(screen.getByTestId("lower"));

    expect(onPageChangeMock).toHaveBeenNthCalledWith(1, 6);

    fireEvent.click(screen.getByTestId("greater"));

    expect(onPageChangeMock).toHaveBeenNthCalledWith(2, 8);

    fireEvent.click(screen.getByText(8));

    expect(onPageChangeMock).toHaveBeenNthCalledWith(3, 8);
  });
});
