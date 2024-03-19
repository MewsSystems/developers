import { formatLanguageCodeToString } from "./formatLanguageCodeToString";
import { NO_VALUE_PLACEHOLDER } from "../constants";

describe("formatLanguageCodeToString", () => {
  it("should return a correctly formatted string if a language code is passed", () => {
    expect(formatLanguageCodeToString("en")).toBe("English");
  });

  it("should return NO_VALUE_PLACEHOLDER if no language code is passed", () => {
    expect(formatLanguageCodeToString(undefined)).toBe(NO_VALUE_PLACEHOLDER);
  });
});
