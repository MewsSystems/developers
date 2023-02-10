import React from "react";
import { MemoryRouter } from "react-router-dom";
import { renderWithProviders } from "../../testing/mockProvider";
import { Paginator } from "./Paginator";

import { mockState } from "../../testing/mockState";

describe("Paginator component", () => {
  test("it should render 6 pagination buttons on the first page", () => {
    const { getAllByRole } = renderWithProviders(
      <MemoryRouter>
        <Paginator />
      </MemoryRouter>,
      {
        preloadedState: {
          moviesList: { ...mockState, activePage: 1 },
        },
      }
    );

    const buttons = getAllByRole("button");
    expect(buttons.length).toBe(6);
  });

  test("each button should have correct value", () => {
    const { getAllByRole } = renderWithProviders(
      <MemoryRouter>
        <Paginator />
      </MemoryRouter>,
      {
        preloadedState: {
          moviesList: { ...mockState, activePage: 2 },
        },
      }
    );

    const buttons = getAllByRole("button");
    const expectedButtonValues = ["Prev", "1", "2", "3", "4", "5", "Next"];
    buttons.forEach((button, i) =>
      expect(button.textContent).toBe(expectedButtonValues[i])
    );
  });

  test("it should render only three numberic buttons and next button when there are only three pages", () => {
    const { getAllByRole } = renderWithProviders(
      <MemoryRouter>
        <Paginator />
      </MemoryRouter>,
      {
        preloadedState: {
          moviesList: { ...mockState, totalPages: 3 },
        },
      }
    );

    const buttons = getAllByRole("button");
    expect(buttons.length).toBe(4);
  });

  test("it should render only one button when there is only one page", () => {
    const { getAllByRole } = renderWithProviders(
      <MemoryRouter>
        <Paginator />
      </MemoryRouter>,
      {
        preloadedState: {
          moviesList: { ...mockState, totalPages: 1 },
        },
      }
    );

    const buttons = getAllByRole("button");
    expect(buttons.length).toBe(1);
  });
});
