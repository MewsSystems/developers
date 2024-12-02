import { describe, expect, it, vi } from "vitest";

import { fireEvent, render, screen } from "@testing-library/react";

import "@testing-library/jest-dom";
import { movies } from "../../../tests/handlers/movieHandlers";
import { MovieListItem } from "./MovieListItem";

describe("MovieListItem", () => {
  it("selects movie", () => {
    const setSelectedMock = vi.fn();
    render(<MovieListItem movie={movies[0]} setSelected={setSelectedMock} />)

    const selectButton = screen.getByText("Select");
    
    expect(selectButton).toBeInTheDocument();

    fireEvent.click(selectButton);

    expect(setSelectedMock).toBeCalledWith(movies[0].id);
  })
})