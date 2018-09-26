// @flow strict

import { loadState, saveState } from "../localStorage";

describe("localStorage", () => {
  test("should work", () => {
    const data = { name: "Filip" };
    // @$FlowTest
    saveState(data);
    expect(loadState()).toEqual(data);
  });
});
