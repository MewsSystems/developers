// @flow strict
import { cleanup } from "react-testing-library";
import React from "react";
import List from "../List";
import renderWithRedux from "../../utils/testsUtils";
import store from "../../records/storeState";

afterEach(cleanup);

jest.mock("../Rate", () => ({ before, current }: { before: ?number, current: number }) => (
  <span>
    {before} {current}
  </span>
));

describe("List", () => {
  test("render", () => {
    const { baseElement } = renderWithRedux(<List />, { initialState: store });
    expect(baseElement).toMatchSnapshot();
  });
});
