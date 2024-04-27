import SearchInput from '@/components/SearchInput';
import { fireEvent, render, screen } from '@testing-library/react';

describe('SearchInput', () => {
  test('should call onSearch callback with correct value when search button is clicked', () => {
    const onChangeMock = vi.fn();

    render(<SearchInput onChange={onChangeMock} placeholder="Search items" />);

    const inputElement = screen.getByPlaceholderText('Search items');

    fireEvent.change(inputElement, { target: { value: 'Test search' } });

    expect(onChangeMock).toHaveBeenCalledTimes(1);
    expect(onChangeMock).toHaveBeenCalledWith('Test search');
  });
});
