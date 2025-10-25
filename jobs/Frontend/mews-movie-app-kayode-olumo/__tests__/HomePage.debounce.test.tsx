import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import HomePage from "@/app/page";

// Mock the movie service
jest.mock("@/lib/services/movieService", () => ({
  searchMovies: jest.fn(),
}));

describe("Search Debounce", () => {
  let mockSearchMovies: jest.MockedFunction<any>;

  beforeEach(async () => {
    const { searchMovies } = await import("@/lib/services/movieService");
    mockSearchMovies = searchMovies as jest.MockedFunction<any>;
    mockSearchMovies.mockResolvedValue({
      results: [{ id: 1, title: "Test Movie" }],
      total_pages: 1,
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should call searchMovies after typing", async () => {
    render(<HomePage />);
    
    const input = screen.getByTestId("search-input");
    await userEvent.type(input, "test");
    
    // Wait for debounce
    await new Promise(resolve => setTimeout(resolve, 400));
    
    expect(mockSearchMovies).toHaveBeenCalledWith("test", 1);
  });
});