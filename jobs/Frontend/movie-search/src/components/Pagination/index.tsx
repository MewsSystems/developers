import React, { useState } from 'react';

interface PaginationProps {
    currentPage: number;
    totalPages: number;
    onPageChange: (pageNumber: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({ currentPage, totalPages, onPageChange }) => {
    const [currentPageState, setCurrentPageState] = useState(currentPage);

    const goToPage = (pageNumber: number) => {
        if (pageNumber >= 1 && pageNumber <= totalPages) {
            setCurrentPageState(pageNumber);
            onPageChange(pageNumber);
        }
    };

    const goToPreviousPage = () => {
        if (currentPageState > 1) {
            goToPage(currentPageState - 1);
        }
    };

    const goToNextPage = () => {
        if (currentPageState < totalPages) {
            goToPage(currentPageState + 1);
        }
    };

    return (
        <div>
            <button onClick={goToPreviousPage} disabled={currentPageState === 1}>
                Previous
            </button>
            <span>{currentPageState} of {totalPages}</span>
            <button onClick={goToNextPage} disabled={currentPageState === totalPages}>
                Next
            </button>
        </div>
    );
};

export default Pagination;
