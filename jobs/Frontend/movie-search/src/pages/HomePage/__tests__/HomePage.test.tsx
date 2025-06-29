import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { vi, describe, it, expect, beforeEach } from "vitest";
import "@testing-library/jest-dom";
import { HomePage } from "../index";
import { MemoryRouter } from "react-router";

vi.mock("../../../hooks/useMoviesQueries", () => ({
  usePopularMoviesQuery: () => ({
    data: {
      pages: [
        {
          page: 1,
          total_pages: 2,
          total_results: 2,
          results: [
            {
              id: 1,
              title: "Test Movie 1",
              adult: false,
              backdrop_path: null,
              genre_ids: [],
              original_language: "en",
              original_title: "Test Movie 1",
              overview: "Test overview",
              poster_path: null,
              popularity: 100,
              release_date: "2023-01-01",
              video: false,
              vote_average: 7.5,
              vote_count: 1000,
            },
          ],
        },
      ],
    },
    fetchNextPage: vi.fn(),
    hasNextPage: true,
    isFetching: false,
    isFetchingNextPage: false,
    status: "success",
  }),
}));

vi.mock("../../../hooks/useSearchMoviesQuery", () => ({
  useSearchMoviesQuery: () => ({
    data: {
      pages: [
        {
          page: 1,
          total_pages: 1,
          total_results: 1,
          results: [
            {
              id: 2,
              title: "Search Result",
              adult: false,
              backdrop_path: null,
              genre_ids: [],
              original_language: "en",
              original_title: "Search Result",
              overview: "Search overview",
              poster_path: null,
              popularity: 50,
              release_date: "2023-01-01",
              video: false,
              vote_average: 6.5,
              vote_count: 500,
            },
          ],
        },
      ],
    },
    fetchNextPage: vi.fn(),
    hasNextPage: false,
    isFetching: false,
    isFetchingNextPage: false,
    status: "success",
  }),
}));

vi.mock("../../../hooks/useDebounce", () => ({
  useDebounce: (value: string) => value,
}));

vi.mock("../../../hooks/useInfiniteScroll", () => ({
  useInfiniteScroll: vi.fn(),
}));

function createTestQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });
}

const wrapper = ({ children }: { children: React.ReactNode }) => {
  const queryClient = createTestQueryClient();
  return (
    <QueryClientProvider client={queryClient}>
      <MemoryRouter>{children}</MemoryRouter>
    </QueryClientProvider>
  );
};

describe("HomePage", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("renders the home page with search input and heading", () => {
    render(<HomePage />, { wrapper });

    expect(screen.getByDisplayValue("")).toBeInTheDocument();

    expect(screen.getByText("List of movies")).toBeInTheDocument();
  });

  it("allows user to type in search input", async () => {
    render(<HomePage />, { wrapper });

    const searchInput = screen.getByDisplayValue("");

    fireEvent.change(searchInput, { target: { value: "test search" } });

    await waitFor(() => {
      expect(searchInput).toHaveValue("test search");
    });
  });

  it("displays movies list component", () => {
    render(<HomePage />, { wrapper });

    expect(screen.getByText("Loading more…")).toBeInTheDocument();
  });

  it("updates search input value when user types", async () => {
    render(<HomePage />, { wrapper });

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
    render(<HomePage />, { wrapper });

    const searchInput = screen.getByDisplayValue("");

    expect(searchInput).toHaveAttribute("name", "searchField");

    expect(searchInput).toHaveAttribute("placeholder", "Search...");
  });
});
