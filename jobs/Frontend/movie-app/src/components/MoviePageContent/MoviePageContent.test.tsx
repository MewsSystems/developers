import React from "react";
import { render, screen, waitFor } from "@testing-library/react";
import { Provider } from "react-redux";
import MoviePageContent from "./MoviePageContent";
import { BrowserRouter } from "react-router-dom";
import { store } from "../../state/store";
import { popularMoviesMockedReponse } from "../../state/movies/movies.mock";

describe("MoviePageContent", () => {
  it("should fetch popular movies and display it", async () => {
    global.fetch = jest.fn().mockResolvedValue({
      json: jest.fn().mockResolvedValue(popularMoviesMockedReponse),
    });

    render(
      <BrowserRouter>
        <Provider store={store}>
          <MoviePageContent />
        </Provider>
      </BrowserRouter>
    );

    await waitFor(() => {
      expect(screen.getByTestId("movie-card")).toBeDefined();
    });
  });
});
