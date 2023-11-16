import { describe, expect, it, vi } from "vitest";

import { fireEvent, render, screen } from "@testing-library/react";

import "@testing-library/jest-dom";
import { SearchInput } from "./SearchInput";

describe("SearchInput", () => {
  it("accepts input", () => {
    const setSearchMock = vi.fn();
    render(<SearchInput setSearch={setSearchMock}/>);

    const input = screen.getByTestId("search-input");
    expect(input).toBeInTheDocument();

    fireEvent.change(input, { target: { value: "test" } });
    expect(setSearchMock).toHaveBeenCalledWith("test");
  })
})