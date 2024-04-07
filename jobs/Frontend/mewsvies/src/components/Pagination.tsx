import { PaginationProps } from "../interfaces";

export const Pagination = ({ page, total_pages, onPreviousPage, onNextPage }: PaginationProps) => {
    return (
        <div>
            <button onClick={onPreviousPage} disabled={page === 1}>
                Previous Page
            </button>
            <span>
                Page {page} of {total_pages}
            </span>
            <button onClick={onNextPage} disabled={page === total_pages}>
                Next Page
            </button>
        </div>
    );
};
