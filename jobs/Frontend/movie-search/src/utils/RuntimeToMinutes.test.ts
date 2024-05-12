import { runtimeToHoursMinutes } from "./RuntimeToMinutes";

describe("RuntimeToMinutes", () => {
  it("converts a runtime of 120 to 2h 0m", () => {
    const runtime = 120;

    const minutes = runtimeToHoursMinutes(runtime);

    expect(minutes).toBe("2H 0M");
  });

  it("converts a runtime of 150 to 2h 30m", () => {
    const runtime = 150;

    const minutes = runtimeToHoursMinutes(runtime);

    expect(minutes).toBe("2H 30M");
  });

  it("converts a runtime of 30 to 0h 30m", () => {
    const runtime = 30;

    const minutes = runtimeToHoursMinutes(runtime);

    expect(minutes).toBe("0H 30M");
  });
});
