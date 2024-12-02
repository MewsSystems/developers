import getYear from "./GetYear";

describe("getYear", () => {
  it("returns the year from a date string", () => {
    const date = "2022-01-01";
    const year = getYear(date);
    expect(year).toBe(2022);
  });

  it("returns the year from a date-time string", () => {
    const dateTime = "2022-12-31T23:59:59";
    const year = getYear(dateTime);
    expect(year).toBe(2022);
  });
});
