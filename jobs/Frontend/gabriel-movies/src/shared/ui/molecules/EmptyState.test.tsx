import { describe, it, expect } from "vitest";
import { screen } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { EmptyState } from "./EmptyState";

describe("EmptyState", () => {
  it("renders title only", () => {
    render(<EmptyState title="No results" />);

    expect(screen.getByText("No results")).toBeInTheDocument();
  });

  it("renders subtitle when provided", () => {
    render(<EmptyState title="No results" subtitle="Try another search" />);

    expect(screen.getByText("No results")).toBeInTheDocument();
    expect(screen.getByText("Try another search")).toBeInTheDocument();
  });
});
