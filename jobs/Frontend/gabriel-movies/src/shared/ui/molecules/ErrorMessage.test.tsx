import { describe, it, expect } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { ErrorMessage } from "./ErrorMessage";

describe("ErrorMessage", () => {
  it("renders title and message", () => {
    render(<ErrorMessage message="Network error" />);

    expect(screen.getByText("Something went wrong")).toBeInTheDocument();
    expect(screen.getByText("Network error")).toBeInTheDocument();
  });
});
