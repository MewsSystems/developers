import { fireEvent, render, screen } from '@testing-library/react';
import TextInput from '@/components/TextInput';

describe('TextInput', () => {
  test('should render with placeholder and handle onChange event', () => {
    const onChangeMock = vi.fn();

    render(<TextInput placeholder="Enter your name" onChange={onChangeMock} />);

    const inputElement = screen.getByPlaceholderText('Enter your name');

    fireEvent.change(inputElement, { target: { value: 'John Doe' } });

    expect(onChangeMock).toHaveBeenCalledTimes(1);
    expect(onChangeMock).toHaveBeenCalledWith(expect.any(Object));
    expect(onChangeMock.mock.calls[0][0].target.value).toBe('John Doe');
  });
});
