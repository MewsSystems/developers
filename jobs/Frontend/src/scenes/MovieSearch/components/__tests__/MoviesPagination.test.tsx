import { render, screen } from "@testing-library/react";
import MoviesPagination from "@/scenes/MovieSearch/components/MoviesPagination";
import { usePathname, useSearchParams } from "next/navigation";

jest.mock("next/navigation", () => ({
  usePathname: jest.fn(),
  useSearchParams: jest.fn(),
  useRouter: jest.fn(),
}));
const usePathnameMock = usePathname as jest.Mock;
const useSearchParamsMock = useSearchParams as jest.Mock;

describe("MoviesPagination", () => {
  beforeEach(() => {
    (usePathnameMock as jest.Mock).mockReturnValue("/");
    (useSearchParamsMock as jest.Mock).mockReturnValue(new URLSearchParams());
  });
  it("doesn't display pagination when there's only one page", () => {
    render(<MoviesPagination totalPages={1} currentPage={1} />);
    expect(screen.queryByRole("navigation")).not.toBeInTheDocument();
  });

  it("displays pagination when there are multiple pages", () => {
    render(<MoviesPagination totalPages={5} currentPage={2} />);
    expect(screen.getByRole("navigation")).toBeVisible();
    expect(screen.getByRole("link", { name: "1" })).toBeVisible();
    expect(screen.getByRole("link", { name: "2" })).toBeVisible();
    expect(screen.queryByRole("link", { name: "3" })).not.toBeInTheDocument();
    expect(screen.queryByRole("link", { name: "4" })).not.toBeInTheDocument();
    expect(screen.getByRole("link", { name: "5" })).toBeVisible();
    expect(
      screen.getByRole("link", { name: "Go to previous page" }),
    ).toBeVisible();
    expect(screen.getByRole("link", { name: "Go to next page" })).toBeVisible();
  });

  it("doesn't display the previous button on first page", () => {
    render(<MoviesPagination totalPages={5} currentPage={1} />);
    expect(screen.getByRole("link", { name: "1" })).toBeVisible();
    expect(screen.getByRole("link", { name: "5" })).toBeVisible();
    expect(
      screen.queryByRole("link", { name: "Go to previous page" }),
    ).not.toBeInTheDocument();
    expect(screen.getByRole("link", { name: "Go to next page" })).toBeVisible();
  });

  it("doesn't display the next button on last page", () => {
    render(<MoviesPagination totalPages={5} currentPage={5} />);
    expect(screen.getByRole("link", { name: "1" })).toBeVisible();
    expect(screen.getByRole("link", { name: "5" })).toBeVisible();
    expect(
      screen.getByRole("link", { name: "Go to previous page" }),
    ).toBeVisible();
    expect(
      screen.queryByRole("link", { name: "Go to next page" }),
    ).not.toBeInTheDocument();
  });

  it("uses the correct hrefs for the pagination links", () => {
    render(<MoviesPagination totalPages={5} currentPage={3} />);
    expect(
      screen.getByRole("link", { name: "Go to previous page" }),
    ).toHaveAttribute("href", "/?page=2");
    expect(screen.getByRole("link", { name: "1" })).toHaveAttribute(
      "href",
      "/?page=1",
    );
    expect(screen.getByRole("link", { name: "3" })).toHaveAttribute(
      "href",
      "/?page=3",
    );
    expect(screen.getByRole("link", { name: "5" })).toHaveAttribute(
      "href",
      "/?page=5",
    );
    expect(
      screen.getByRole("link", { name: "Go to next page" }),
    ).toHaveAttribute("href", "/?page=4");
  });
});
