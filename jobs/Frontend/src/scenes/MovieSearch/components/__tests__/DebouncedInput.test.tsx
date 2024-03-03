import { render, screen, fireEvent } from "@testing-library/react";
import DebouncedInput from "@/scenes/MovieSearch/components/DebouncedInput";

const debouncedOnChange = jest.fn();
const onChange = jest.fn();
describe("DebouncedInput", () => {
  beforeEach(() => {
    jest.useFakeTimers();
    jest.clearAllMocks();
  });

  afterEach(() => {
    jest.clearAllTimers();
    jest.useRealTimers();
  });

  it("calls debouncedOnChange after delay", async () => {
    render(
      <DebouncedInput debouncedOnChange={debouncedOnChange} delay={1000} />,
    );
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "test" },
    });
    expect(debouncedOnChange).not.toHaveBeenCalled();
    jest.advanceTimersByTime(1000);
    expect(debouncedOnChange).toHaveBeenCalledWith("test");
  });

  it("call debouncedOnChange immediately if delay is 0", async () => {
    render(<DebouncedInput debouncedOnChange={debouncedOnChange} delay={0} />);
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "test" },
    });
    jest.advanceTimersByTime(0);
    expect(debouncedOnChange).toHaveBeenCalledWith("test");
  });

  it("calls debouncedOnChange immediately if threshold is reached", async () => {
    render(
      <DebouncedInput
        debouncedOnChange={debouncedOnChange}
        delay={1000}
        delayedThreshold={2}
      />,
    );
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "t" },
    });
    expect(debouncedOnChange).not.toHaveBeenCalled();
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "te" },
    });
    expect(debouncedOnChange).not.toHaveBeenCalled();
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "tes" },
    });
    expect(debouncedOnChange).toHaveBeenCalledWith("tes");
  });

  it("calls debouncedOnChange immediately when the input is cleared", async () => {
    render(
      <DebouncedInput debouncedOnChange={debouncedOnChange} delay={1000} />,
    );
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "test" },
    });
    expect(debouncedOnChange).not.toHaveBeenCalled();
    jest.advanceTimersByTime(1000);
    expect(debouncedOnChange).toHaveBeenCalledWith("test");
    debouncedOnChange.mockClear();
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "" },
    });
    expect(debouncedOnChange).toHaveBeenCalledWith("");
  });

  it("calls onChange prop", async () => {
    render(
      <DebouncedInput
        debouncedOnChange={debouncedOnChange}
        onChange={onChange}
      />,
    );
    fireEvent.change(screen.getByRole("textbox"), {
      target: { value: "test" },
    });
    expect(onChange).toHaveBeenCalled();
    expect(debouncedOnChange).not.toHaveBeenCalled();
  });
});
