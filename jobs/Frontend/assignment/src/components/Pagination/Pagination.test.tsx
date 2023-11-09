import { render, fireEvent } from "@/tests";
import { Pagination } from "./Pagination";

describe("Pagination", () => {
  const onChange = vi.fn();

  afterEach(() => {
    vi.clearAllMocks();
  });

  it("should render the correct page numbers", () => {
    const { getByText } = render(
      <Pagination currentPage={3} totalPages={10} onChange={onChange} />,
    );

    expect(getByText("3 of 10")).toBeInTheDocument();
  });

  it("should call onChange with the correct page number when a button is clicked", () => {
    const { getByTestId } = render(
      <Pagination currentPage={3} totalPages={10} onChange={onChange} />,
    );

    const prevButton = getByTestId("prev-page-btn");
    const nextButton = getByTestId("next-page-btn");
    const firstButton = getByTestId("first-page-btn");
    const lastButton = getByTestId("last-page-btn");

    fireEvent.click(prevButton);
    expect(onChange).toHaveBeenCalledWith(2);

    fireEvent.click(nextButton);
    expect(onChange).toHaveBeenCalledWith(4);

    fireEvent.click(firstButton);
    expect(onChange).toHaveBeenCalledWith(1);

    fireEvent.click(lastButton);
    expect(onChange).toHaveBeenCalledWith(10);
  });

  it("should not call onChange when a disabled button is clicked", () => {
    const { getByTestId } = render(
      <Pagination currentPage={1} totalPages={1} onChange={onChange} />,
    );

    const prevButton = getByTestId("prev-page-btn");
    const nextButton = getByTestId("next-page-btn");
    const firstButton = getByTestId("first-page-btn");
    const lastButton = getByTestId("last-page-btn");

    fireEvent.click(prevButton);
    fireEvent.click(firstButton);
    fireEvent.click(nextButton);
    fireEvent.click(lastButton);

    expect(onChange).not.toHaveBeenCalled();
  });

  it("should disable the previous and first page buttons on the first page", () => {
    const { getByTestId } = render(
      <Pagination currentPage={1} totalPages={10} onChange={onChange} />,
    );

    const prevButton = getByTestId("first-page-btn");
    const firstButton = getByTestId("prev-page-btn");

    expect(prevButton).toBeDisabled();
    expect(firstButton).toBeDisabled();
  });

  it("should disable the next and last page buttons on the last page", () => {
    const { getByTestId } = render(
      <Pagination currentPage={10} totalPages={10} onChange={onChange} />,
    );

    const nextButton = getByTestId("next-page-btn");
    const lastButton = getByTestId("last-page-btn");

    expect(nextButton).toBeDisabled();
    expect(lastButton).toBeDisabled();
  });
});
