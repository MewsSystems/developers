// @flow
import { render, cleanup } from "react-testing-library";
import React from "react";
import ErrorBox from "../ErrorBox";

afterEach(cleanup);

jest.mock("react-icons/fa", () => ({ FaExclamationTriangle: jest.fn(() => "Mocked React Icon") }));

describe("ErrorBox", () => {
  test("render", () => {
    const { baseElement } = render(<ErrorBox error={new Error("test error")} />);
    expect(baseElement).toMatchSnapshot();
  });
});
