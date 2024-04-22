import { tmdb } from "@/app/services/tmdb/tmdb.data";
import { fireEvent, render, screen } from "@/testing-utils/render.utils";
import { describe, expect, it, vi } from "vitest";
import { Search } from "../Search.page";
import { mockMovie } from "@/testing-utils/fixtures.utils";

describe("Search", () => {
  const mockResponse = {
    page: 1,
    total_pages: 3,
    total_results: 1,
    results: [mockMovie],
  };
  const availablePages = [1, 2, 3];
  const baseQuery = "matrix";

  it("renders the search input field and the initial landing", () => {
    render(<Search />);

    expect(screen.getByText("Welcome")).toBeInTheDocument();
    expect(
      screen.getByText(
        "Millions of movies, TV shows and people to discover. Explore now",
      ),
    ).toBeInTheDocument();
    expect(screen.getByLabelText("Search movie input")).toBeInTheDocument();
  });

  it("searches for movie results if query params are included in the url", async () => {
    const mockSearchEndpoint = vi
      .spyOn(tmdb.search, "movies" as never)
      .mockResolvedValue(mockResponse);

    render(<Search />, {
      currentRoute: `/search?q=${baseQuery}&page=1`,
    });

    await screen.findByText(mockMovie.title);

    expect(mockSearchEndpoint).toHaveBeenCalledWith({
      query: baseQuery,
      page: 1,
    });
  });

  it("searches movie results after a inputting a query and a small delay", async () => {
    const query = "Test query";
    const mockSearchEndpoint = vi
      .spyOn(tmdb.search, "movies" as never)
      .mockResolvedValue(mockResponse);

    render(<Search />);

    const input = screen.getByLabelText("Search movie input");

    // Advance the timers to trigger the search
    vi.useFakeTimers();
    fireEvent.change(input, { target: { value: query } });
    vi.advanceTimersByTimeAsync(300);
    vi.useRealTimers();

    await screen.findByText(mockMovie.title);

    expect(mockSearchEndpoint).toHaveBeenCalledWith({
      query,
      page: 1,
    });
  });

  it("renders the pagination links", async () => {
    vi.spyOn(tmdb.search, "movies" as never).mockResolvedValue(mockResponse);

    render(<Search />, {
      currentRoute: `/search?q=${baseQuery}`,
    });

    await screen.findByText(mockMovie.title);

    expect(screen.getByTestId("search-pagination")).toBeInTheDocument();
    availablePages.forEach((page) =>
      expect(
        screen.getByRole("link", { name: page.toString() }),
      ).toHaveAttribute("href", `/?page=${page}&q=${baseQuery}`),
    );
  });

  it("renders the empty results page if no results are available", async () => {
    vi.spyOn(tmdb.search, "movies" as never).mockResolvedValue({
      page: 1,
      total_pages: 1,
      total_results: 0,
      results: [],
    });

    render(<Search />, {
      currentRoute: "/search?q=non-existent-movie",
    });

    await screen.findByText("No results");
    screen.getByText("There were no matches for your search term.");
  });
});
