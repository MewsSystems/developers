import { describe, it, expect } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { Loader } from "./Loader";

describe("Loader", () => {
  it("renders status with default label", () => {
    render(<Loader />);

    const status = screen.getByRole("status");
    expect(status).toBeInTheDocument();
    expect(status.querySelector('[aria-hidden="true"]')).toBeTruthy();
    expect(screen.getByText("Loading...")).toBeInTheDocument();
  });

  it("renders a custom label", () => {
    render(<Loader label="Searching movies..." />);

    expect(screen.getByText("Searching movies...")).toBeInTheDocument();
  });
});
