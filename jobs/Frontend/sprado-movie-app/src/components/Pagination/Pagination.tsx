import React from "react";
import { generatePageNumbers } from "../../utils/pagination-utils/generate-page-numbers";

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export const Pagination = ({
  currentPage,
  totalPages,
  onPageChange,
}: PaginationProps) => {
  const handlePageClick = (page: number | string) => {
    if (typeof page === "number") {
      onPageChange(page);
    }
  };

  const handlePrev = () => {
    if (currentPage > 1) onPageChange(currentPage - 1);
  };

  const handleNext = () => {
    if (currentPage < totalPages) onPageChange(currentPage + 1);
  };

  return (
    <div className="flex justify-center items-center mt-8 space-x-4">
      <button
        data-testId="prev-page-button"
        onClick={handlePrev}
        disabled={currentPage === 1}
        className={`px-3 py-2 rounded-full ${
          currentPage === 1
            ? "bg-gray-700 text-gray-500 cursor-not-allowed"
            : "bg-purple-600 text-white hover:bg-purple-500"
        }`}
      >
        &larr;
      </button>

      {generatePageNumbers(currentPage, totalPages).map((page, index) => (
        <button
          data-testid="page-button"
          key={index}
          onClick={() => handlePageClick(page)}
          disabled={page === "..."}
          className={`px-3 py-2 rounded-md ${
            page === currentPage
              ? "bg-purple-600 text-white"
              : page === "..."
                ? "bg-transparent text-gray-500 cursor-default"
                : "bg-gray-800 text-white hover:bg-purple-500"
          }`}
        >
          {page}
        </button>
      ))}

      <button
        data-testId="next-page-button"
        onClick={handleNext}
        disabled={currentPage === totalPages}
        className={`px-3 py-2 rounded-full ${
          currentPage === totalPages
            ? "bg-gray-700 text-gray-500 cursor-not-allowed"
            : "bg-purple-600 text-white hover:bg-purple-500"
        }`}
      >
        &rarr;
      </button>
    </div>
  );
};
