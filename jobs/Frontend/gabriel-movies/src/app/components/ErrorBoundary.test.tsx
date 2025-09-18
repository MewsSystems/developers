import { describe, it, expect } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { ErrorBoundary } from "./ErrorBoundary";

const ErrorChild = ({ message = "There is an error" }) => {
  throw new Error(message);
};

describe("ErrorBoundary", () => {
  it("renders children when there is no error", () => {
    render(
      <ErrorBoundary>
        <div data-testid="ok">Child rendered</div>
      </ErrorBoundary>
    );

    expect(screen.getByTestId("ok")).toHaveTextContent("Child rendered");
  });

  it("renders fallback with the error message", () => {
    render(
      <ErrorBoundary>
        <ErrorChild message="There is an error" />
      </ErrorBoundary>
    );

    expect(screen.getByText(/there is an error/i)).toBeInTheDocument();
  });

  it("renders default message when the error has no message", () => {
    render(
      <ErrorBoundary>
        <ErrorChild />
      </ErrorBoundary>
    );

    expect(screen.getByText(/something went wrong/i)).toBeInTheDocument();
  });
});
