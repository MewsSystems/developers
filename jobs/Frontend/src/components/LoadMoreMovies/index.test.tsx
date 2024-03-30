import { LoadMoreMovies } from ".";
import { fireEvent, render, screen } from "../../test/utils";

describe("LoadMoreMovies", () => {
  it("button changes its state based on loading more movies", async () => {
    render(<LoadMoreMovies />, {
      state: {
        movies: {
          search: {
            results: [],
            page: 0,
            total_pages: 3,
            loading: false,
          },
          query: "",
          selectedMovie: null,
        },
      },
    });
    const loadMoreButton = screen.getByText(/load more/i);
    fireEvent.click(loadMoreButton);
    expect(loadMoreButton.textContent).toEqual("Loading...");
    expect(loadMoreButton).toHaveAttribute("disabled");
  });
});
