import { expect, test, describe } from "vitest";
import { calculateDuration } from "./utils";

describe("utils", () => {
  test("calculates the duration correctly", () => {
    const testCalculation = calculateDuration(62);
    expect(testCalculation).toEqual({ hours: 1, minutes: 2 });
  });
});
