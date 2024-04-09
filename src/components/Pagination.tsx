import styled from "styled-components";
import { PaginationProps } from "../interfaces";

const Button = styled.button`
    background-color: var(--btn-primary);
    border: 1px solid var(--btn-primary);
    border-radius: 15px;
    color: var(--white);
    font-family: "Axiforma-Regular", sans-serif;
    font-size: 0.875rem;
    padding: 0.25rem 1.25rem;
    transition: all 0.25s ease-in;

    &:hover {
        color: var(--btn-primary);
        background-color: var(--white);
        border-color: var(--btn-primary-hover);
        transition: all 0.25s ease-in;
    }

    &:disabled {
        border: none;
        background-color: var(--btn-secondary);
        color: var(--sand);
        cursor: not-allowed;
    }
`;

export const Pagination = ({ page, total_pages, onPreviousPage, onNextPage }: PaginationProps) => {
    return (
        <nav className="max-w-md flex w-full justify-between items-center my-4">
            <Button
                onClick={onPreviousPage}
                disabled={page === 1}
                className={`px-3 py-1 rounded ${page === 1 ? "text-gray-300 cursor-not-allowed" : ""}`}
                rel="prev"
            >
                Previous
            </Button>
            <p className="mt-1 text-sm text-gray-500">
                Page <span>{page}</span> of <span>{total_pages}</span>
            </p>
            <Button
                onClick={onNextPage}
                disabled={page === total_pages}
                className={`px-3 py-1 rounded ${page === total_pages ? "text-gray-300 cursor-not-allowed" : ""}`}
                rel="next"
            >
                Next
            </Button>
        </nav>
    );
};
