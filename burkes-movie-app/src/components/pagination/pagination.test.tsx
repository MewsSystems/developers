import { fireEvent, render, screen } from '@testing-library/react';
import { Mock, beforeEach, describe, expect, it, vi } from 'vitest';

import { Pagination } from './Pagination';

describe('Pagination', () => {
  let setCurrentPageMock: Mock;

  beforeEach(() => {
    setCurrentPageMock = vi.fn();
  });

  it('renders correctly with less than 4 pages', () => {
    render(
      <Pagination
        numberOfPages={3}
        currentPage={1}
        setCurrentPage={setCurrentPageMock}
      />
    );

    // 3 pages, plus NEXT and PREVIOUS buttons
    const items = screen.getAllByRole('listitem');
    expect(items).toHaveLength(5);
  });

  it('renders correctly with exactly 4 pages', async () => {
    render(
      <Pagination
        numberOfPages={4}
        currentPage={1}
        setCurrentPage={setCurrentPageMock}
      />
    );

    const items = screen.getAllByRole('listitem');
    expect(items).toHaveLength(6);
  });

  it('increments currentPage when the NEXT button is clicked', () => {
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

  it('displays ellipsis when there are more than 4 pages and current page is within the first three pages', () => {
    render(
      <Pagination
        numberOfPages={6}
        currentPage={2}
        setCurrentPage={setCurrentPageMock}
      />
    );

    const ellipsis = screen.getByText('...');
    expect(ellipsis).toBeInTheDocument();
  });

  it('displays ellipsis when current page is in the middle of the page range', () => {
    render(
      <Pagination
        numberOfPages={6}
        currentPage={4}
        setCurrentPage={setCurrentPageMock}
      />
    );

    const ellipses = screen.getAllByText('...');
    expect(ellipses.length).toBe(1);
  });
});
