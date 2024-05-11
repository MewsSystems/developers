// minutesToHoursMinutes.test.ts
import { minutesToHoursMinutes } from "./minutesToHours";

describe("minutesToHoursMinutes", () => {
  test("converts 0 minutes to [0, 0]", () => {
    expect(minutesToHoursMinutes(0)).toEqual([0, 0]);
  });

  test("converts 60 minutes to [1, 0]", () => {
    expect(minutesToHoursMinutes(60)).toEqual([1, 0]);
  });

  test("converts 90 minutes to [1, 30]", () => {
    expect(minutesToHoursMinutes(90)).toEqual([1, 30]);
  });

  test("converts 121 minutes to [2, 1]", () => {
    expect(minutesToHoursMinutes(121)).toEqual([2, 1]);
  });

  test("converts 360 minutes to [6, 0]", () => {
    expect(minutesToHoursMinutes(360)).toEqual([6, 0]);
  });
});
