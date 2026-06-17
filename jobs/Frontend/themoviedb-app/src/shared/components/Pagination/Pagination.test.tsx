import { screen } from '@testing-library/react';
import { expect } from 'vitest';
import { render } from '../../../tests/utils';
import Pagination from './Pagination';

describe('Pagination', () => {
    it('should render Pagination', () => {
        render(<Pagination page={1} totalPages={5} onChange={() => null} />);
        const pageText = screen.getByText('Page 1 of 5');
        expect(pageText).toBeInTheDocument();
    });

    it('should have previous page and next page buttons', () => {
        render(<Pagination page={1} totalPages={5} onChange={() => null} />);
        const prevPageBtn = screen.getByTestId('prev-page');
        const nextPageBtn = screen.getByTestId('next-page');
        expect(prevPageBtn).toBeInTheDocument();
        expect(nextPageBtn).toBeInTheDocument();
    });

    it('should have previous page button disabled if no previous page ', () => {
        render(<Pagination page={1} totalPages={5} onChange={() => null} />);
        const prevPageBtn = screen.getByTestId('prev-page');
        expect(prevPageBtn).toBeDisabled();
    });

    it('should have next page button disabled if no next page ', () => {
        render(<Pagination page={5} totalPages={5} onChange={() => null} />);
        const nextPageBtn = screen.getByTestId('next-page');
        expect(nextPageBtn).toBeDisabled();
    });

    it('should execute onChange fn if previous or next page is clicked', () => {
        const onClickFn = vi.fn();
        render(<Pagination page={1} totalPages={5} onChange={onClickFn} />);
        screen.getByTestId('next-page').click();
        expect(onClickFn).toHaveBeenCalled();
    });
});
