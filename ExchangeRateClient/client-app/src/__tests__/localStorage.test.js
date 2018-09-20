// @flow strict

import { loadState, saveState } from "../localStorage";

describe("localStorage", () => {
  test("should work", () => {
    const data = { name: "Filip" };
    saveState(data);
    expect(loadState()).toEqual(data);
  });
});
