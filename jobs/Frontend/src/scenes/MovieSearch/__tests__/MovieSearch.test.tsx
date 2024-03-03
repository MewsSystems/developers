import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import MovieSearch from "@/scenes/MovieSearch/MovieSearch";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import fetchMovies from "@/scenes/MovieSearch/services/fetchMovies";
import { testMovie } from "@/jest/fakeData/movie";

jest.mock("next/navigation", () => ({
  usePathname: jest.fn(),
  useRouter: jest.fn(),
  useSearchParams: jest.fn(),
}));
const usePathnameMock = usePathname as jest.Mock;
const useRouterMock = useRouter as jest.Mock;
const useSearchParamsMock = useSearchParams as jest.Mock;
const pushMock = jest.fn();

jest.mock("@/scenes/MovieSearch/services/fetchMovies");
const fetchMoviesMock = fetchMovies as jest.Mock;

describe("MovieSearch", () => {
  beforeEach(() => {
    (usePathnameMock as jest.Mock).mockReturnValue("/");
    (useSearchParamsMock as jest.Mock).mockReturnValue(new URLSearchParams());
    (useRouterMock as jest.Mock).mockReturnValue({
      push: pushMock,
    });
    fetchMoviesMock.mockResolvedValue({
      page: 1,
      results: [testMovie],
      total_pages: 5,
      total_results: 1,
    });
    jest.useFakeTimers();
    jest.clearAllMocks();
  });

  afterEach(() => {
    jest.clearAllTimers();
    jest.useRealTimers();
  });

  it("should render static content", () => {
    render(<MovieSearch />);

    expect(screen.getByRole("heading", { name: "Movie Search" })).toBeVisible();
    expect(screen.getByRole("textbox")).toBeVisible();
  });

  it("should add query param after user input", () => {
    render(<MovieSearch />);
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "test" },
    });
    jest.runAllTimers();
    expect(pushMock).toHaveBeenCalledWith("/?query=test");

    pushMock.mockClear();
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "" },
    });
    expect(pushMock).toHaveBeenCalledWith("/?");
  });

  it("should call fetchMovies with query param", async () => {
    (useSearchParamsMock as jest.Mock).mockReturnValue(
      new URLSearchParams("query=test"),
    );
    render(<MovieSearch />);
    await waitFor(() =>
      expect(fetchMoviesMock).toHaveBeenCalledWith("test", 1),
    );
  });

  it("should call fetchMovies with query and page param", async () => {
    (useSearchParamsMock as jest.Mock).mockReturnValue(
      new URLSearchParams("query=test&page=2"),
    );
    render(<MovieSearch />);
    await waitFor(() =>
      expect(fetchMoviesMock).toHaveBeenCalledWith("test", 2),
    );
  });

  it("should render movies", async () => {
    (useSearchParamsMock as jest.Mock).mockReturnValue(
      new URLSearchParams("query=test&page=1"),
    );
    render(<MovieSearch />);
    expect(await screen.findByText("Test Movie")).toBeVisible();
    expect(screen.getByRole("link", { name: "1" })).toBeVisible();
    expect(screen.getByRole("link", { name: "1" })).toHaveAttribute(
      "href",
      "/?query=test&page=1",
    );
  });

  it("should render info about no movies found", async () => {
    (useSearchParamsMock as jest.Mock).mockReturnValue(
      new URLSearchParams("query=test&page=1"),
    );
    fetchMoviesMock.mockResolvedValue({
      page: 1,
      results: [],
      total_pages: 0,
      total_results: 0,
    });
    render(<MovieSearch />);
    expect(await screen.findByText("No movies found")).toBeVisible();
  });
});
