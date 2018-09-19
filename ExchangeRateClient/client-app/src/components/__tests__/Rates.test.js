// @flow strict

import { render, cleanup } from "react-testing-library";
import React from "react";
import Rate from "../Rate";

afterEach(cleanup);

describe("Rates", () => {
  test("render", () => {
    const { baseElement } = render(<Rate current={10} before={23} />);
    expect(baseElement).toMatchSnapshot();
  });

  test("if current and before values are same should render the Snowflake icon", () => {
    const { getByTestId } = render(<Rate current={10} before={10} />);
    expect(getByTestId(`icon`)).toMatchSnapshot();
  });

  test("if the current is more than the before value", () => {
    const { getByTestId } = render(<Rate current={11} before={10} />);
    expect(getByTestId(`icon`)).toMatchSnapshot();
  });

  test("if the before is more than the current value", () => {
    const { getByTestId } = render(<Rate current={1} before={10} />);
    expect(getByTestId("icon")).toMatchSnapshot();
  });
});
