import { BrowserRouter } from "react-router-dom";
import { render, screen } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { test, describe, expect, vi } from "vitest";
import userEvent from "@testing-library/user-event";

import SearchInput from "./SearchInput";
const queryClient = new QueryClient();

describe("SearchInput", () => {
  test("search ts ", async () => {
    const handleChange = vi.fn();
    render(
      <QueryClientProvider client={queryClient}>
        <SearchInput
          handleChange={handleChange}
          placeholder={""}
          query={null}
        />
      </QueryClientProvider>,
      { wrapper: BrowserRouter }
    );
    const searchBar = screen.getByRole("searchbox");
    expect(searchBar).toBeDefined();
    await userEvent.type(searchBar, "lion");
    expect(handleChange).toBeCalled();
  });
});
