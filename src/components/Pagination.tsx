'use client';

import { AiFillCaretLeft, AiFillCaretRight } from 'react-icons/ai';
import { MouseEventHandler } from 'react';

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  search: string;
  onPageChange: (page: number) => void;
}

const getPageRange = (currentPage: number, totalPages: number): number[] => {
  return Array.from({ length: 5 }, (_, i) => i + Math.max(1, currentPage - 2)).filter(
    (p) => p <= totalPages
  );
};

export function Pagination({ currentPage, totalPages, onPageChange }: PaginationProps) {
  const createPageHandler: (page: number) => MouseEventHandler<HTMLButtonElement> =
    (page) => (e) => {
      e.preventDefault();
      onPageChange(page);
    };

  const pageRange = getPageRange(currentPage, totalPages);

  return (
    <nav aria-label="Pagination" className="flex gap-2 mt-4 flex-wrap justify-between items-center">
      <div className="w-16 flex justify-start">
        {currentPage > 1 && (
          <button
            onClick={createPageHandler(currentPage - 1)}
            className="text-purple-800 hover:underline focus:underline cursor-pointer"
            aria-label="Previous page"
          >
            <AiFillCaretLeft aria-hidden="true" />
          </button>
        )}
      </div>

      <div className="flex gap-1 items-center">
        {pageRange.map((p) =>
          p === currentPage ? (
            <button
              key={p}
              aria-current="page"
              disabled
              tabIndex={-1}
              className="px-2 py-1 rounded bg-purple-950 text-white pointer-events-none"
            >
              {p}
            </button>
          ) : (
            <button
              key={p}
              onClick={createPageHandler(p)}
              aria-label={`Page ${p}`}
              className="px-2 py-1 rounded transition text-purple-800 hover:underline focus:underline cursor-pointer"
            >
              {p}
            </button>
          )
        )}

        {!pageRange.includes(totalPages) && (
          <span className="flex gap-2">
            <span aria-hidden="true">…</span>
            <button
              onClick={createPageHandler(totalPages)}
              className="text-purple-800 hover:underline focus:underline cursor-pointer"
              aria-label={`Page ${totalPages}`}
            >
              {totalPages}
            </button>
          </span>
        )}
      </div>

      <div className="w-4 flex justify-end">
        {currentPage < totalPages && (
          <button
            onClick={createPageHandler(currentPage + 1)}
            className="text-purple-800 hover:underline focus:underline cursor-pointer"
            aria-label="Next page"
          >
            <AiFillCaretRight aria-hidden="true" />
          </button>
        )}
      </div>
    </nav>
  );
}
