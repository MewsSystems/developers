import React from "react";
import { renderWithProviders } from "../../public-api";

import { mockState } from "../../testing/mockState";
import { ResultBar } from "./ResultBar";

describe("Paginator component", () => {
  test("it should render 6 pagination buttons on the first page", () => {
    const { baseElement } = renderWithProviders(<ResultBar />, {
      preloadedState: {
        moviesList: { ...mockState, results: 10 },
      },
    });

    const title = baseElement.querySelector("h2");
    expect(title?.textContent).toBe("10 Results");
  });
});
