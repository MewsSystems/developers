import { describe, it, expect } from "vitest";
import { formatDate } from "./formatDate";

describe("formatDate", () => {
  it("formats date to en-GB long form", () => {
    expect(formatDate("2014-11-07")).toBe("7 November 2014");
  });
});
