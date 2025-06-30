import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router";
import { describe, it, expect } from "vitest";
import App from "../App";

describe("App", () => {
  it("renders the HomePage for the root route", () => {
    render(
      <MemoryRouter initialEntries={["/"]}>
        <App />
      </MemoryRouter>
    );

    expect(screen.getByText("List of movies")).toBeInTheDocument();
  });

  it("renders the DetailPage for the /movie/:id route", () => {
    render(
      <MemoryRouter initialEntries={["/movie/123"]}>
        <App />
      </MemoryRouter>
    );

    // we have only "Back button" present as no other data are loaded
    expect(screen.getByRole("button")).toBeInTheDocument();
  });
});
