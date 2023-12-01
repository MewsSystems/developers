import { render, screen, waitFor, act } from "@testing-library/react";
import SearchInput from "../SearchInput";
import userEvent from "@testing-library/user-event";

describe("SearchInput", () => {
    test("renders without errors", () => {
        render(<SearchInput onChange={() => {}} aria-label="Search" />);
        expect(screen.getByRole("search")).toBeInTheDocument();
    });

    test("calls onChange callback when input value changes", async () => {
        const handleChange = jest.fn();

        render(<SearchInput onChange={handleChange} aria-label="Search" />);

        const input = screen.getByRole("textbox");
        await userEvent.type(input, "test");

        await waitFor(() => {
            expect(handleChange).toHaveBeenCalledWith("test");
        });
    });

    test("resets input value when reset button is clicked", async () => {
        const handleChange = jest.fn();

        render(<SearchInput onChange={handleChange} aria-label="Search" />);

        const input = screen.getByRole("textbox");

        await userEvent.type(input, "test");

        const resetButton = screen.getByRole("button");

        await userEvent.click(resetButton);

        expect(input).toHaveValue("");
    });
});
