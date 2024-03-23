import { fireEvent, render, screen } from '@testing-library/react';
import { describe, expect, it, vi } from 'vitest';

import { Pagination } from './Pagination';

describe('Pagination', () => {
  it('renders the correct number of items', () => {
    const numberOfPages = 5;

    render(
      <Pagination
        numberOfPages={numberOfPages}
        currentPage={1}
        setCurrentPage={() => {}}
      />
    );

    const items = screen.getAllByTestId('pagination-item');
    expect(items.length).toBe(numberOfPages);
  });

  it('increments currentPage when the NEXT button is clicked', () => {
    const setCurrentPageMock = vi.fn();

    render(
      <Pagination
        numberOfPages={5}
        currentPage={1}
        setCurrentPage={setCurrentPageMock}
      />
    );

    fireEvent.click(screen.getByText('NEXT'));

    const incrementFunction = setCurrentPageMock.mock.calls[0][0];
    expect(incrementFunction(1)).toBe(2);
  });

  it('decrements currentPage when the PREV button is clicked', () => {
    const setCurrentPageMock = vi.fn();

    render(
      <Pagination
        numberOfPages={5}
        currentPage={2}
        setCurrentPage={setCurrentPageMock}
      />
    );

    fireEvent.click(screen.getByText('PREVIOUS'));

    const decrementFunction = setCurrentPageMock.mock.calls[0][0];
    expect(decrementFunction(2)).toBe(1);
  });
});
