import React from 'react';
import { render, screen } from '@testing-library/react';
import { Pagination } from './Pagination';

describe('pagination component', () => {
  const renderPagination = (page: number, totalPages: number) => {
    render(<Pagination page={page} totalPages={totalPages} onPageChanged={() => null} />);
  }

  it('renders correctly 5 pages 1-5', () => {
    renderPagination(2, 6);
    expect(screen.queryByText('1')).toBeInTheDocument();
    expect(screen.queryByText('2')).toBeInTheDocument();
    expect(screen.queryByText('3')).toBeInTheDocument();
    expect(screen.queryByText('4')).toBeInTheDocument();
    expect(screen.queryByText('5')).toBeInTheDocument();
    expect(screen.queryByText('6')).not.toBeInTheDocument();    
  });

  it('renders correctly 5 pages 2-6', () => {
    renderPagination(4, 8);
    expect(screen.queryByText('1')).not.toBeInTheDocument();
    expect(screen.queryByText('2')).toBeInTheDocument();
    expect(screen.queryByText('3')).toBeInTheDocument();
    expect(screen.queryByText('4')).toBeInTheDocument();
    expect(screen.queryByText('5')).toBeInTheDocument();
    expect(screen.queryByText('6')).toBeInTheDocument();
    expect(screen.queryByText('7')).not.toBeInTheDocument();
  });

  it('renders correctly less than 5 pages', () => {
    renderPagination(3, 3);
    expect(screen.queryByText('1')).toBeInTheDocument();
    expect(screen.queryByText('2')).toBeInTheDocument();
    expect(screen.queryByText('3')).toBeInTheDocument();
    expect(screen.queryByText('4')).not.toBeInTheDocument();
  });
});
