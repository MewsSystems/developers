import { render, screen, fireEvent, cleanup } from '@testing-library/react';
import { DebouncedInput } from './DebouncedInput';

describe('DebouncedInput', () => {
  const placeholder = 'Search...';
  const ariaLabel = 'Search input';
  const debounceDelay = 500;
  const initialValue = 'Hello';

  let handleChange: ReturnType<typeof vi.fn>;

  beforeEach(() => {
    vi.useFakeTimers();
    handleChange = vi.fn();
  });

  afterEach(() => {
    vi.runOnlyPendingTimers();
    vi.clearAllTimers();
    vi.useRealTimers();
    vi.resetAllMocks();
    cleanup();
  });

  it('renders with initial props and accessible label', () => {
    render(
      <DebouncedInput
        value={initialValue}
        onChange={handleChange}
        placeholder={placeholder}
        ariaLabel={ariaLabel}
      />
    );

    const input = screen.getByRole('searchbox', { name: ariaLabel });
    expect(input).toBeInTheDocument();
    expect(input).toHaveValue(initialValue);

    const placeholderInput = screen.getByPlaceholderText(placeholder);
    expect(placeholderInput).toBeInTheDocument();
  });

  it('updates internal state and debounces onChange call', async () => {
    render(<DebouncedInput value="" onChange={handleChange} debounceDelay={debounceDelay} />);

    const input = screen.getByRole('searchbox');
    fireEvent.change(input, { target: { value: 'test' } });

    expect(input).toHaveValue('test');
    expect(handleChange).not.toHaveBeenCalled();

    vi.advanceTimersByTime(debounceDelay);

    expect(handleChange).toHaveBeenCalledWith('test');
  });

  it('resets internal state when `value` prop changes externally', () => {
    const { rerender } = render(<DebouncedInput value="foo" onChange={handleChange} />);

    const input = screen.getByRole('searchbox');
    expect(input).toHaveValue('foo');

    rerender(<DebouncedInput value="bar" onChange={handleChange} />);
    expect(input).toHaveValue('bar');
  });

  it('cleans up the debounce on unmount', () => {
    const { unmount } = render(
      <DebouncedInput value="" onChange={handleChange} debounceDelay={debounceDelay} />
    );

    const input = screen.getByRole('searchbox');
    fireEvent.change(input, { target: { value: 'debounced' } });

    unmount();

    vi.advanceTimersByTime(debounceDelay);
    expect(handleChange).not.toHaveBeenCalled();
  });

  it('supports custom class name and aria-describedby', () => {
    render(
      <DebouncedInput
        value=""
        onChange={handleChange}
        className="custom-input"
        ariaDescribedBy="desc"
      />
    );

    const input = screen.getByRole('searchbox');
    expect(input).toHaveClass('custom-input');
    expect(input).toHaveAttribute('aria-describedby', 'desc');
  });
});
