import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import HomePage from "@/app/page";

jest.mock("@/lib/services/movieService", () => ({
  searchMovies: jest.fn(),
}));

describe("HomePage search debounce", () => {
  let mockSearchMovies: jest.MockedFunction<any>;

  beforeEach(async () => {
    const { searchMovies } = await import("@/lib/services/movieService");
    mockSearchMovies = searchMovies as jest.MockedFunction<any>;
    mockSearchMovies.mockResolvedValue({
      results: [{ id: 1, title: "Test Movie" }],
      total_pages: 1,
    });
    jest.useRealTimers();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("calls searchMovies once after debounce with the final query", async () => {
    render(<HomePage />);

    await Promise.resolve();
    mockSearchMovies.mockClear();

    const input = screen.getByPlaceholderText(/search for movies/i);
    await userEvent.type(input, "test");

    await new Promise((r) => setTimeout(r, 400));

    expect(mockSearchMovies).toHaveBeenCalledTimes(1);
    expect(mockSearchMovies).toHaveBeenCalledWith("test", 1);
  }, 15000);
});