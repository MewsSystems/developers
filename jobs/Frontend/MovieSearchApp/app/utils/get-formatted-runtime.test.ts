import { describe, expect, test } from "vitest"
import { getFormattedRuntime } from "~/utils/get-formatted-runtime"

describe("getFormattedRuntime", () => {
  test("returns null when minutes is null", () => {
    expect(getFormattedRuntime(null)).toBe(null)
  })

  test("returns formatted runtime when hours is 0", () => {
    expect(getFormattedRuntime(30)).toBe("30m")
  })

  test("returns formatted runtime when minutes is 0", () => {
    expect(getFormattedRuntime(120)).toBe("2h")
  })

  test("returns formatted runtime when hours and minutes are not 0", () => {
    expect(getFormattedRuntime(150)).toBe("2h 30m")
  })
})
