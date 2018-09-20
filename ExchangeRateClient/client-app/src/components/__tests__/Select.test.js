// @flow
import { render, cleanup } from "react-testing-library";
import React from "react";
import Select from "react-select";
import MySelect from "../Select";
import { renderWithRedux } from "../../utils/testsUtils";
import store from "../../records/storeState";

afterEach(cleanup);

jest.mock("react-select", () => props => {
  // check if right pros are sent to the Select component
  expect(props).toMatchSnapshot();
  return null;
});

describe("Select", () => {
  test("test if props are set properly", () => {
    const { baseElement } = renderWithRedux(<MySelect />, { initialState: store });
    expect(baseElement).toBeDefined();
  });
});
