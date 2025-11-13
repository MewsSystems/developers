import { fireEvent, render, screen } from '@testing-library/react';
import { describe, expect, it, vi } from 'vitest';

import { PaginationItem } from './PaginationItem';

describe('PaginationItem', () => {
  it('renders the page number', () => {
    render(
      <PaginationItem page={1} currentPage={1} setCurrentPage={() => {}} />
    );

    expect(screen.getByText('1')).toBeInTheDocument();
  });

  it('has active class when page equals currentPage', () => {
    render(
      <PaginationItem page={1} currentPage={1} setCurrentPage={() => {}} />
    );

    const element = screen.getByText('1');
    expect(element.parentElement?.className.includes('active'));
  });

  it('does not have an active class when page does not equal current page', () => {
    render(
      <PaginationItem page={1} currentPage={2} setCurrentPage={() => {}} />
    );

    const element = screen.getByText('1');
    expect(!element.parentElement?.className.includes('active'));
  });

  it('calls setCurrentPage with the correct page when clicked', () => {
    const setCurrentPageMock = vi.fn();

    render(
      <PaginationItem
        page={3}
        currentPage={2}
        setCurrentPage={setCurrentPageMock}
      />
    );

    fireEvent.click(screen.getByText('3'));
    expect(setCurrentPageMock).toHaveBeenCalledWith(3);
  });
});
