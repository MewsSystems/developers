import { PaginationProps } from "../interfaces";

export const Pagination = ({ page, total_pages, onPreviousPage, onNextPage }: PaginationProps) => {
    return (
        <nav className="max-w-md flex w-full justify-between items-center my-4">
            <button
                onClick={onPreviousPage}
                disabled={page === 1}
                className={`px-3 py-1 rounded ${
                    page === 1
                        ? "bg-gray-100 text-gray-300 cursor-not-allowed"
                        : "bg-gray-200 text-gray-700"
                }`}
                rel="prev"
            >
                Previous
            </button>
            <p className="mt-1 text-sm text-gray-500">
                Page <span>{page}</span> of <span>{total_pages}</span>
            </p>
            <button
                onClick={onNextPage}
                disabled={page === total_pages}
                className={`px-3 py-1 rounded ${
                    page === total_pages
                        ? "bg-gray-100 text-gray-300 cursor-not-allowed"
                        : "bg-gray-200 text-gray-700"
                }`}
                rel="next"
            >
                Next
            </button>
        </nav>
    );
};
