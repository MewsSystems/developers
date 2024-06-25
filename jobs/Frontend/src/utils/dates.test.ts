// formatDate.test.ts
import { formatDate } from "./dates"

describe("formatDate", () => {
  it("should return an empty string if the input is an empty string", () => {
    expect(formatDate("")).toBe("Invalid Date")
  })

  it("should format a valid date string correctly", () => {
    expect(formatDate("2024-06-25")).toBe("Jun 25, 2024")
    expect(formatDate("2000-01-01")).toBe("Jan 1, 2000")
  })

  it("should handle various valid date formats", () => {
    expect(formatDate("2024-06-25T12:00:00Z")).toBe("Jun 25, 2024")
    expect(formatDate("06/25/2024")).toBe("Jun 25, 2024")
    expect(formatDate("2024/06/25")).toBe("Jun 25, 2024")
  })
})
