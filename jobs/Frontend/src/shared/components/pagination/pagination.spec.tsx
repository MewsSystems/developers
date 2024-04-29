import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import Pagination from './pagination';

describe('Pagination component', () => {
  it('renders correctly', () => {
    const count = 10;
    const page = 1;
    const onPageChange = jest.fn();

    const { getByRole } = render(
      <Pagination count={count} page={page} onPageChange={onPageChange} />,
    );

    expect(getByRole('navigation')).toBeInTheDocument();
  });

  it('calls onPageChange callback when page is changed', () => {
    const count = 10;
    const page = 1;
    const onPageChange = jest.fn();

    const { getByRole } = render(
      <Pagination count={count} page={page} onPageChange={onPageChange} />,
    );

    fireEvent.click(getByRole('button', { name: 'Go to next page' }));

    expect(onPageChange).toHaveBeenCalledWith(2);
  });
});
