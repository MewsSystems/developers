import { describe, it, expect } from "vitest";
import { roundVoteAverageToPercent } from "./roundVoteAverageToPercent";

describe("roundVoteAverageToPercent", () => {
  it.each([
    [8.6, 86],
    [6.04, 60],
    [0, 0],
  ])("rounds %s -> %s", (input, expected) => {
    expect(roundVoteAverageToPercent(input)).toBe(expected);
  });
});
