import { render } from "@testing-library/react";
import { describe, expect, it } from "vitest";

import App from "./App";

describe("App", () => {
  it("renders the App component", () => {
    render(<App />);

    const rootElement = document.getElementById("root")!;
    const inputElement = document.getElementsByTagName("input")!;

    expect(rootElement).toBeDefined();
    expect(inputElement[0]).toBeInTheDocument();
    expect(inputElement.length).toBe(1);
    // screen.debug(); // prints out the jsx in the App component unto the command line
  });
});
