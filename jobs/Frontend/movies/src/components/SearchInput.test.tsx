import { render } from "@testing-library/react";
import { SearchInput } from "./SearchInput";

describe("SearchInput", () => {
  it("should reander the input", () => {
    const screen = render(<SearchInput placeholder="test" />);

    expect(screen.getByPlaceholderText("test")).toBeInTheDocument();
  });
});
