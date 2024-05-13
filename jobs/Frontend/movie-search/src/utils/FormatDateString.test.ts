import { formatDateString } from "./FormatDateString";

describe("formatDateString", () => {
  it("formats a date string in the UK format", () => {
    const dateString = "2022-01-01";
    const formattedDate = formatDateString(dateString);
    expect(formattedDate).toBe("1 January 2022");
  });

  it("formats a date-time string in the UK format", () => {
    const dateTimeString = "2022-12-31T23:59:59";
    const formattedDate = formatDateString(dateTimeString);
    expect(formattedDate).toBe("31 December 2022");
  });
});
