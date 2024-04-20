import { fireEvent, render } from "@testing-library/react";
import { describe, expect, it, vi } from "vitest";

import ErrorMessage from "./ErrorMessage";

const error = "Test error";
const handleClear = vi.fn();

describe("ErrorMessage", () => {
  it("renders error custom message and button", () => {
    const { getByText, getByRole } = render(
      <ErrorMessage error={error} handleClear={handleClear} />,
    );

    expect(getByText(error)).toBeInTheDocument();
    expect(getByRole("button", { name: "Go to homepage" })).toBeInTheDocument();
    // Simulate click on the button
    fireEvent.click(getByRole("button", { name: "Go to homepage" }));
    // Check if the handleClear function is called
    expect(handleClear).toHaveBeenCalled();
  });

  it("renders default error message when error prop is not provided", () => {
    const { getByText } = render(<ErrorMessage handleClear={handleClear} />);

    // Check if the default error message is rendered
    expect(
      getByText("Oops! Something went wrong. Please try again later."),
    ).toBeInTheDocument();
  });
});
