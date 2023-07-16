import React from "react";
import { render, fireEvent, screen, act } from "@testing-library/react";
import { InputSearch } from "./InputSearch";
import "@testing-library/jest-dom/extend-expect";

/**
 * Tests InputSearch component
 * NOTE: Made a couple of tests here as an example of how would unit/component testing be done
 *
 * This is how would unit/component tests be done for each component inside of a project
 *
 */
describe("InputSearch component test", () => {
  const onDebounceMock = jest.fn<string, [string]>((value) => value);
  const onChangeMock = jest.fn<string, [string]>((value) => value);
  const onEnterMock = jest.fn<string, [string]>((value) => value);

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should call onDebounce after debounce time with the proper value", () => {
    jest.useFakeTimers();

    render(<InputSearch onDebounce={onDebounceMock} debounceTime={1000} />);

    const inputElement = screen.getByRole("textbox");

    expect(inputElement).toBeInTheDocument();
    expect(inputElement).toHaveValue("");

    fireEvent.change(inputElement, { target: { value: "Apple" } });

    expect(onDebounceMock).not.toHaveBeenCalled();

    act(() => jest.runAllTimers());

    expect(onDebounceMock).toHaveBeenCalledWith("Apple");
    expect(onDebounceMock.mock.results[0].value).toBe("Apple");
    expect(inputElement).toHaveValue("Apple");
  });

  it("calls onChange properly", () => {
    render(<InputSearch onChange={onChangeMock} />);

    const inputElement = screen.getByRole("textbox");

    fireEvent.change(inputElement, { target: { value: "Apple" } });

    expect(onChangeMock).toHaveBeenCalledWith("Apple");
    expect(onChangeMock.mock.results[0].value).toBe("Apple");

    expect(inputElement).toHaveValue("Apple");
  });

  it("calls onEnter properly", () => {
    render(<InputSearch onEnter={onEnterMock} />);
    const inputElement = screen.getByRole("textbox");

    fireEvent.keyDown(inputElement, { key: "Enter" });

    expect(onEnterMock).toHaveBeenCalledWith("");
    expect(onEnterMock.mock.results[0].value).toBe("");

    fireEvent.change(inputElement, { target: { value: "Apple" } });
    fireEvent.keyDown(inputElement, { key: "Enter" });
    expect(onEnterMock.mock.results[1].value).toBe("Apple");
  });
});
