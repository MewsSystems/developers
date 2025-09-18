import { describe, it, expect, vi } from "vitest";
import { lazy } from "react";
import { screen, act } from "@testing-library/react";
import { render } from "@/test-utils/render";
import "@/test-utils/mocks/Loader.mock";
import { SuspenseBoundary } from "./SuspenseBoundary";

describe("SuspenseBoundary", () => {
  it("shows loader while page is pending", async () => {
    vi.useFakeTimers();
    const LazyPage = lazy(() =>
      new Promise<{ default: React.ComponentType }>((resolve) =>
        setTimeout(() => resolve({ default: () => <div>Page loaded</div> }), 0)
      )
    );

    render(
      <SuspenseBoundary label="Please wait">
        <LazyPage />
      </SuspenseBoundary>
    );

    expect(screen.getByTestId("loader")).toHaveTextContent("Please wait");

    await act(async () => {
      await vi.advanceTimersByTimeAsync(0);
    });

    expect(screen.queryByTestId("loader")).not.toBeInTheDocument();
    expect(screen.getByText("Page loaded")).toBeInTheDocument();
    vi.useRealTimers();
  });
});
