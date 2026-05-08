import { dateFormatter } from "./dateFormatter"; // Make sure to adjust the import path accordingly

describe("dateFormatter", () => {
  test("formats a date string correctly", () => {
    const dateString = "2024-05-10T12:00:00Z"; // Example date string
    const formattedDate = dateFormatter(dateString);
    expect(formattedDate).toBe("May 10, 2024");
  });

  test("handles invalid date string gracefully", () => {
    const dateString = "Invalid Date"; // Example invalid date string
    const formattedDate = dateFormatter(dateString);
    expect(formattedDate).toBe("Invalid Date");
  });

  test("handles empty string gracefully", () => {
    const dateString = ""; // Empty string
    const formattedDate = dateFormatter(dateString);
    expect(formattedDate).toBe("Invalid Date");
  });
});
