'use client';

import { AiFillCaretLeft, AiFillCaretRight } from 'react-icons/ai';
import { MouseEventHandler } from 'react';

interface PaginationProps extends React.HTMLAttributes<HTMLElement> {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  readonly?: boolean;
}

const getPageRange = (currentPage: number, totalPages: number): number[] => {
  return Array.from({ length: 5 }, (_, i) => i + Math.max(1, currentPage - 2)).filter(
    (p) => p <= totalPages
  );
};

export function Pagination({
  currentPage,
  totalPages,
  onPageChange,
  readonly = false,
  ...rest
}: PaginationProps) {
  const buttonBase = 'px-2 py-1 rounded transition text-purple-800 hover:underline focus:underline';
  const buttonCursor = readonly ? 'cursor-not-allowed' : 'cursor-pointer';

  const readonlyProps = {
    disabled: readonly || undefined,
    tabIndex: readonly ? -1 : undefined,
  };

  const createPageHandler: (page: number) => MouseEventHandler<HTMLButtonElement> =
    (page) => (e) => {
      e.preventDefault();
      if (!readonly) onPageChange(page);
    };

  const pageRange = getPageRange(currentPage, totalPages);

  return (
    <nav
      aria-label="Pagination"
      className={`flex gap-2 flex-wrap justify-between items-center transition-opacity ${
        readonly ? 'opacity-60 select-none' : ''
      }`}
      {...rest}
    >
      <div className="w-4 flex justify-start">
        {currentPage > 1 && (
          <button
            onClick={createPageHandler(currentPage - 1)}
            className={`${buttonBase} ${buttonCursor}`}
            aria-label="Previous page"
            {...readonlyProps}
          >
            <AiFillCaretLeft aria-hidden="true" />
          </button>
        )}
      </div>

      <div className="flex gap-1 items-center">
        {pageRange.map((p) =>
          p === currentPage ? (
            <button
              key={`${currentPage}-${p}`}
              aria-current="page"
              disabled
              tabIndex={-1}
              className="px-2 py-1 rounded bg-purple-950 text-white pointer-events-none cursor-not-allowed"
            >
              {p}
            </button>
          ) : (
            <button
              key={`${currentPage}-${p}`}
              onClick={createPageHandler(p)}
              aria-label={`Page ${p}`}
              className={`${buttonBase} ${buttonCursor}`}
              {...readonlyProps}
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
              className={`${buttonBase} ${buttonCursor}`}
              aria-label={`Page ${totalPages}`}
              {...readonlyProps}
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
            className={`${buttonBase} ${buttonCursor}`}
            aria-label="Next page"
            {...readonlyProps}
          >
            <AiFillCaretRight aria-hidden="true" />
          </button>
        )}
      </div>
    </nav>
  );
}
