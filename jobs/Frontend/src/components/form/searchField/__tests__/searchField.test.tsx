import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { SearchField } from "../searchField";
import { SearchFieldProps } from "../types";
import { AppProvider } from "@/context";

const defaultProps: SearchFieldProps = {
  loading: false,
  onChange: jest.fn(),
};

function TestComponent() {
  return (
    <AppProvider>
      <SearchField {...defaultProps} />
    </AppProvider>
  );
}

describe("Search field", () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should display clear button when user types", async () => {
    render(<TestComponent />);

    const field = screen.getByRole("textbox");

    await userEvent.type(field, "dune");

    const clearInput = screen.getByRole("button");

    expect(clearInput).toBeInTheDocument();
  });

  it("should NOT display clear button when field empty", async () => {
    render(<TestComponent />);

    const clearInput = screen.queryByRole("button");

    expect(clearInput).not.toBeInTheDocument();
  });

  it("should hide clear button when user removes search term", async () => {
    render(<TestComponent />);

    const field = screen.getByRole("textbox");

    await userEvent.type(field, "dune");

    const clearInput = screen.getByRole("button");

    expect(clearInput).toBeInTheDocument();

    await userEvent.clear(screen.getByRole("textbox"));

    expect(screen.queryByRole("button")).not.toBeInTheDocument();
  });

  it("should clear input when clear button pressed", async () => {
    render(<TestComponent />);

    const field = screen.getByRole<HTMLInputElement>("textbox");

    await userEvent.type(field, "dune");

    expect(field.value).toEqual("dune");

    const clearInput = screen.getByRole("button");

    await userEvent.click(clearInput);

    expect(field.value).toEqual("");
  });

  it("should hide clear input button when clear button pressed", async () => {
    render(<TestComponent />);

    const field = screen.getByRole<HTMLInputElement>("textbox");

    await userEvent.type(field, "dune");

    expect(field.value).toEqual("dune");

    const clearInput = screen.getByRole("button");

    await userEvent.click(clearInput);

    expect(screen.queryByRole("button")).not.toBeInTheDocument();
  });
});
