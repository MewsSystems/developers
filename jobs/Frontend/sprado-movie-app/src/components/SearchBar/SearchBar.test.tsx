import React from "react";
import { render } from "@testing-library/react";
import { SearchBar } from "./SearchBar";

describe("SearchBar", () => {
  it("renders input field with placeholder", () => {
    render(<SearchBar value="" onChange={() => {}} onReset={() => {}} />);

    const input = document.querySelector("input");
    expect(input).toBeInTheDocument();
    expect(input).toHaveAttribute("placeholder", "Search for a movie...");
  });

  it('renders "✕" button when value is not empty', () => {
    render(<SearchBar value="Batman" onChange={() => {}} onReset={() => {}} />);

    const closeButton = document.querySelector("button");
    expect(closeButton).toBeInTheDocument();
    expect(closeButton).toHaveTextContent("✕");
  });
});
