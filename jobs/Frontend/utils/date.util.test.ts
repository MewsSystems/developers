import { getYear } from "./date.util";

// Simple unit test for demonstration purposes
describe("date.util.ts", () => {
  it("parse year from string", () => {
    expect(getYear("2024-05-05")).toBe("2024");
  });
});
