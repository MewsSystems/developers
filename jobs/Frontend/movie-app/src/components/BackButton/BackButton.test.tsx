import { MemoryRouter, Routes, Route } from "react-router-dom";
import { render, screen } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { test, describe, expect } from "vitest";
import userEvent from "@testing-library/user-event";

import BackButton from "./BackButton";
const queryClient = new QueryClient();

describe("BackButton", () => {
  test("clicking on the button takes the user back ", async () => {
    render(
      <QueryClientProvider client={queryClient}>
        <MemoryRouter initialEntries={["/previous-page", "/current-page"]}>
          <BackButton />
          <Routes>
            <Route path="/previous-page" element={<p>previous page</p>} />
            <Route path="/current-page" element={<p>current page</p>} />
          </Routes>
        </MemoryRouter>
      </QueryClientProvider>
    );
    expect(screen.getByText(/current page/i)).toBeDefined();
    const backButton = screen.getByRole("button", {
      name: /< back/i,
    });
    const currentPage = screen.queryByText(/current page/i);
    expect(currentPage).toBeDefined();
    await userEvent.click(backButton);
    const previousPage = screen.getByText(/previous page/i);
    expect(previousPage).toBeDefined();
  });
});
