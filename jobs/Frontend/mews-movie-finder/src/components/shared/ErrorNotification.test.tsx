import { describe, expect, it } from "vitest";

import { render, screen } from "@testing-library/react";

import "@testing-library/jest-dom";
import { ErrorNotification } from "./ErrorNotification";

describe("ErrorNotification", () => {
  it("should render default message", () => {
    render(<ErrorNotification />);
    const notification = screen.getByTestId("error-message");
    expect(notification).toBeInTheDocument();
    expect(notification.textContent).toBe(
      "Something went wrong, please try again later"
    );
  });

  it("should render custom message", () => {
    render(<ErrorNotification message="custom" />);
    const notification = screen.getByTestId("error-message");
    expect(notification).toBeInTheDocument();
    expect(notification.textContent).toBe("custom");
  });
});
