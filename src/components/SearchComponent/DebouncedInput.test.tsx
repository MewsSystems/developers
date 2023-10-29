import { render, fireEvent, waitFor, act } from '@testing-library/react';
import { DebouncedInput } from './DebouncedInput';
import React from 'react';

jest.mock('lodash/debounce', () => (callback: () => void) => {
  return jest.fn(callback);
});

jest.useFakeTimers();
describe('DebouncedInput', () => {
  it('renders with a placeholder', () => {
    const { getByPlaceholderText } = render(
      <DebouncedInput placeholder="Search Placeholder" handleOnChange={jest.fn()} />,
    );
    const input = getByPlaceholderText('Search Placeholder');
    expect(input).toBeTruthy();
  });

  it('triggers handleOnChange after debounce', async () => {
    const handleOnChange = jest.fn();
    const { getByPlaceholderText } = render(<DebouncedInput handleOnChange={handleOnChange} />);
    const input = getByPlaceholderText('Search...');

    fireEvent.change(input, { target: { value: 'test' } });

    jest.advanceTimersByTime(1000);

    await waitFor(() => {
      expect(handleOnChange).toHaveBeenCalledWith('test');
    });
  });

  it('updates input value and triggers handleOnChange', async () => {
    const handleOnChange = jest.fn();
    const { getByPlaceholderText } = render(<DebouncedInput handleOnChange={handleOnChange} />);
    const input = getByPlaceholderText('Search...');

    fireEvent.change(input, { target: { value: 'test' } });

    jest.advanceTimersByTime(1000);

    await waitFor(() => {
      expect(handleOnChange).toHaveBeenCalledWith('test');
    });
  });
});
