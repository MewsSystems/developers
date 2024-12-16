import { generatePageNumbers } from "./generate-page-numbers";

describe("generatePageNumbers", () => {
  it("returns all pages when totalPages is small", () => {
    expect(generatePageNumbers(1, 5)).toEqual([1, 2, 3, 4, 5]);
  });

  it("returns pages with ellipses when currentPage is in the middle", () => {
    expect(generatePageNumbers(5, 10)).toEqual([1, "...", 4, 5, 6, "...", 10]);
  });

  it("returns only the first page if totalPages is 1", () => {
    expect(generatePageNumbers(1, 1)).toEqual([1]);
  });

  it("handles case with currentPage near the start", () => {
    expect(generatePageNumbers(2, 10)).toEqual([1, 2, 3, "...", 10]);
  });

  it("handles case with currentPage near the end", () => {
    expect(generatePageNumbers(9, 10)).toEqual([1, "...", 8, 9, 10]);
  });

  it("returns correct pages when totalPages is 2", () => {
    expect(generatePageNumbers(1, 2)).toEqual([1, 2]);
  });
});
