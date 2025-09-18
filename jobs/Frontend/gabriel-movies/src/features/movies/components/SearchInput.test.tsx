import { describe, it, expect, vi } from "vitest";
import { screen, fireEvent } from "@testing-library/react";
import { render } from "@/test-utils/render";
import { SearchInput } from "./SearchInput";

vi.mock("@/shared/hooks/useDebouncedValue", () => ({
  useDebouncedValue: (v: string) => v
}));

describe("SearchInput", () => {
  it("normalizes and forwards the latest value", () => {
    const onChange = vi.fn();

    render(<SearchInput initialValue="" onChange={onChange} />);

    const input = screen.getByLabelText(/search movies/i);
    fireEvent.change(input, { target: { value: "  Avatar " } });

    expect(onChange).toHaveBeenLastCalledWith("avatar");
  });
});
