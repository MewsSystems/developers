import '@testing-library/jest-dom';
import React from 'react';
import { render, fireEvent } from '@testing-library/react';
import Pagination from '.';

describe('Pagination component', () => {
    it('renders correctly with current page and total pages', () => {
        const { getByText } = render(<Pagination currentPage={1} totalPages={10} onPageChange={() => { }} />);
        expect(getByText('1 of 10')).toBeInTheDocument();
    });

    it('disables Previous button on first page', () => {
        const { getByText } = render(<Pagination currentPage={1} totalPages={10} onPageChange={() => { }} />);
        const previousButton = getByText('Previous');
        expect(previousButton).toBeDisabled();
    });

    it('disables Next button on last page', () => {
        const { getByText } = render(<Pagination currentPage={10} totalPages={10} onPageChange={() => { }} />);
        const nextButton = getByText('Next');
        expect(nextButton).toBeDisabled();
    });

    it('enables Previous button on pages other than the first page', () => {
        const { getByText } = render(<Pagination currentPage={3} totalPages={10} onPageChange={() => { }} />);
        const previousButton = getByText('Previous');
        expect(previousButton).toBeEnabled();
    });

    it('enables Next button on pages other than the last page', () => {
        const { getByText } = render(<Pagination currentPage={5} totalPages={10} onPageChange={() => { }} />);
        const nextButton = getByText('Next');
        expect(nextButton).toBeEnabled();
    });

    it('calls onPageChange with the correct page number when Next button is clicked', () => {
        const onPageChangeMock = jest.fn();
        const { getByText } = render(<Pagination currentPage={3} totalPages={10} onPageChange={onPageChangeMock} />);
        const nextButton = getByText('Next');
        fireEvent.click(nextButton);
        expect(onPageChangeMock).toHaveBeenCalledWith(4);
    });

    it('calls onPageChange with the correct page number when Previous button is clicked', () => {
        const onPageChangeMock = jest.fn();
        const { getByText } = render(<Pagination currentPage={5} totalPages={10} onPageChange={onPageChangeMock} />);
        const previousButton = getByText('Previous');
        fireEvent.click(previousButton);
        expect(onPageChangeMock).toHaveBeenCalledWith(4);
    });
});
