import { beforeEach, describe, expect, it, vi } from "vitest";

import { fireEvent, screen } from "@testing-library/react";
import { renderWithClient } from "../../../tests/testUtils";
import { QueryClient } from "@tanstack/react-query";

import "@testing-library/jest-dom";
import { MovieList } from "./MovieList";
import { movies } from "../../../tests/handlers/movieHandlers";

describe("MovieList", () => {
  const queryClient = new QueryClient();

  const mockSetPage = vi.fn();
  const mockSetSelected = vi.fn();

  beforeEach( () => {
    renderWithClient(
      queryClient,
      <MovieList
        page={{ page: 2, total_pages: 4, total_results: 10, results: movies }}
        setPage={mockSetPage}
        setSelected={mockSetSelected}
      />
    );
  })

  it("page increase fires", () => {
    const increaseButton = screen.getByText(">");
    expect(increaseButton).toBeInTheDocument();
    fireEvent.click(increaseButton);
    expect(mockSetPage).toBeCalledWith(3);

  });

  it("page decrease fires", () => {
    const decreaseButton = screen.getByText("<");
    expect(decreaseButton).toBeInTheDocument();
    fireEvent.click(decreaseButton);
    expect(mockSetPage).toBeCalledWith(1);
  });
});
