import { describe, it, expect } from "vitest";
import { render } from "@/test-utils/render";
import { screen } from "@testing-library/react";
import { UserScore } from "./UserScore";

describe("UserScore", () => {
  it("renders the text", () => {
    render(<UserScore voteAverage={8.0} />);

    const el = screen.getByLabelText("User score 80%");
    expect(el).toBeInTheDocument();
    expect(el).toHaveTextContent("80%");
  });
});
