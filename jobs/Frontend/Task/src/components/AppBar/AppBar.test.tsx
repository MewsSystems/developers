import React from "react";
import { render, screen } from "@testing-library/react";
import { AppBar } from "./AppBar";

describe("<AppBar />", () => {
  it("should display the children being passed", () => {
    render(<AppBar>This is a test</AppBar>);
    expect(screen.getByText(/this is a test/i)).toBeInTheDocument();
  });
});
