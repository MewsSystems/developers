import { vi, describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";

import { SearchInput } from "./SearchInput";

describe("<SearchInput />", () => {
  it("should render with defaultQuery", () => {
    const DEFAULT_QUERY = "Test";
    render(<SearchInput onChange={vi.fn()} defaultQuery={DEFAULT_QUERY} />);

    expect(screen.queryByDisplayValue(DEFAULT_QUERY)).toBeDefined();
  });

  it("should call onChange only after debouncing when typing", async () => {
    vi.useFakeTimers({
      shouldAdvanceTime: true,
    });

    const onChange = vi.fn();
    const DEFAULT_QUERY = "Test";
    render(<SearchInput onChange={onChange} defaultQuery={DEFAULT_QUERY} />);

    const input = screen.getByDisplayValue(DEFAULT_QUERY);

    await userEvent.type(input, "some query");

    expect(onChange).not.toHaveBeenCalled();

    vi.runAllTimers();

    expect(onChange).toHaveBeenCalledWith("Testsome query");

    vi.useRealTimers();
  });
});
