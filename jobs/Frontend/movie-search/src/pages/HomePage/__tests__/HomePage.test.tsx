import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { vi, describe, it, expect, beforeEach } from "vitest";
import "@testing-library/jest-dom";
import { HomePage } from "../index";
import { TestWrapper } from "../../../test-utils/wrappers";
import {
  createPopularMoviesQueryMock,
  createSearchMoviesQueryMock,
} from "../../../mocks/queries";

vi.mock("../../../hooks/useMoviesQueries", () => ({
  usePopularMoviesQuery: () => createPopularMoviesQueryMock(),
}));

vi.mock("../../../hooks/useSearchMoviesQuery", () => ({
  useSearchMoviesQuery: () => createSearchMoviesQueryMock(),
}));

vi.mock("../../../hooks/useDebounce", () => ({
  useDebounce: (value: string) => value,
}));

vi.mock("../../../hooks/useInfiniteScroll", () => ({
  useInfiniteScroll: vi.fn(),
}));

describe("HomePage", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("renders the home page with search input and heading", () => {
    render(<HomePage />, { wrapper: TestWrapper });

    expect(screen.getByDisplayValue("")).toBeInTheDocument();

    expect(screen.getByText("List of movies")).toBeInTheDocument();
  });

  it("allows user to type in search input", async () => {
    render(<HomePage />, { wrapper: TestWrapper });

    const searchInput = screen.getByDisplayValue("");

    fireEvent.change(searchInput, { target: { value: "test search" } });

    await waitFor(() => {
      expect(searchInput).toHaveValue("test search");
    });
  });

  it("displays movies list component", () => {
    render(<HomePage />, { wrapper: TestWrapper });

    expect(screen.getByText("Loading more…")).toBeInTheDocument();
  });

  it("updates search input value when user types", async () => {
    render(<HomePage />, { wrapper: TestWrapper });

    const searchInput = screen.getByDisplayValue("");

    fireEvent.change(searchInput, { target: { value: "t" } });
    fireEvent.change(searchInput, { target: { value: "te" } });
    fireEvent.change(searchInput, { target: { value: "tes" } });
    fireEvent.change(searchInput, { target: { value: "test" } });

    await waitFor(() => {
      expect(searchInput).toHaveValue("test");
    });
  });

  it("has configured attributes", () => {
    render(<HomePage />, { wrapper: TestWrapper });

    const searchInput = screen.getByDisplayValue("");

    expect(searchInput).toHaveAttribute("name", "searchField");

    expect(searchInput).toHaveAttribute("placeholder", "Search...");
  });
});
