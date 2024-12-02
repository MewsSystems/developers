import { fireEvent, render, screen } from "../../../test/utils";
import { SearchInput } from "./SearchInput";

describe("SearchInput", () => {
  it("input changes value and an api call is made", async () => {
    render(<SearchInput />);
    const input = screen.getByPlaceholderText(/Search for a movie/i);
    fireEvent.change(input, { target: { value: "batman" } });
    expect(input).toHaveValue("batman");
  });
});
