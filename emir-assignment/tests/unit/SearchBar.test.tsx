import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { useState } from "react";
import SearchBar from "../../src/components/SearchBar";

function SearchBarHost({ onChangeSpy }: { onChangeSpy?: (v: string) => void }) {
    const [value, setValue] = useState("");
    return (
        <SearchBar
            value={value}
            onChange={(next) => {
                setValue(next);
                onChangeSpy?.(next);
            }}
        />
    );
}

it("updates value and calls onChange", async () => {
    const user = userEvent.setup();
    const spy = vi.fn();

    render(<SearchBarHost onChangeSpy={spy} />);
    const input = screen.getByRole("textbox", { name: /search movies/i });

    await user.type(input, "Inception");

    expect(input).toHaveValue("Inception");
    expect(spy).toHaveBeenCalled();
    expect(spy.mock.calls.at(-1)?.[0]).toBe("Inception");
});
