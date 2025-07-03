'use client';

import { FaAngleLeft, FaAnglesLeft, FaAngleRight, FaAnglesRight } from 'react-icons/fa6';
import { HTMLAttributes } from 'react';

export interface PaginationProps extends HTMLAttributes<HTMLElement> {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  search: string;
  readonly?: boolean;
  disableKeyboardNav?: boolean;
}

const getPageRange = (currentPage: number, totalPages: number): number[] => {
  const range: number[] = [];
  const max = Math.min(totalPages, 5);
  let start = Math.max(1, currentPage - 2);
  const end = Math.min(totalPages, start + max - 1);

  if (end - start < max - 1) {
    start = Math.max(1, end - max + 1);
  }

  for (let i = start; i <= end; i++) {
    range.push(i);
  }
  return range;
};

function buildHref(page: number, search?: string) {
  const params = new URLSearchParams();
  if (search) params.set('search', search);
  if (page > 1) params.set('page', String(page));
  return `?${params.toString()}`;
}

export function Pagination({
  currentPage,
  totalPages,
  onPageChange,
  search,
  readonly = false,
  disableKeyboardNav = false,
  ...rest
}: PaginationProps) {
  const linkBase =
    'px-1 py-0.5 min-w-[2rem] sm:px-2 sm:py-1 sm:min-w-[2.5rem] flex justify-center items-center rounded transition text-purple-800 hover:underline focus:underline';
  const linkCursor = readonly ? 'cursor-not-allowed' : 'cursor-pointer';

  const keyboardNavProps = disableKeyboardNav || readonly ? { tabIndex: -1 } : {};
  const readonlyProps = readonly ? { 'aria-disabled': true } : {};

  const handleLinkClick = (page: number) => (e: React.MouseEvent<HTMLAnchorElement>) => {
    if (readonly) {
      e.preventDefault();
      return;
    }
    if (e.defaultPrevented || e.metaKey || e.altKey || e.ctrlKey || e.shiftKey || e.button !== 0) {
      return;
    }
    e.preventDefault();
    onPageChange(page);
  };

  const pageRange = getPageRange(currentPage, totalPages);

  return (
    <nav
      aria-label="Pagination"
      className={`flex items-center justify-between transition-opacity ${
        readonly ? 'opacity-60 select-none' : ''
      }`}
      {...rest}
    >
      <div className="min-w-8 flex justify-start">
        {currentPage > 1 && (
          <a
            href={buildHref(1, search)}
            aria-label="Go to first page"
            title="Go to first page"
            className={`${linkBase} ${linkCursor}`}
            onClick={handleLinkClick(1)}
            {...keyboardNavProps}
            {...readonlyProps}
          >
            <FaAnglesLeft aria-hidden="true" />
          </a>
        )}
      </div>

      <div className="flex items-center gap-1 sm:gap-2">
        {currentPage > 1 ? (
          <a
            href={buildHref(currentPage - 1, search)}
            aria-label="Previous page"
            title="Previous page"
            className={`${linkBase} ${linkCursor}`}
            onClick={handleLinkClick(currentPage - 1)}
            {...keyboardNavProps}
            {...readonlyProps}
          >
            <FaAngleLeft aria-hidden="true" />
          </a>
        ) : (
          <div className="min-w-8 sm:min-w-10" />
        )}

        <div className="flex gap-1 sm:gap-2">
          {pageRange.map((p) =>
            p === currentPage ? (
              <a
                key={`${currentPage}-${p}`}
                aria-current="page"
                tabIndex={-1}
                className="px-1 py-0.5 min-w-[2rem] sm:px-2 sm:py-1 sm:min-w-[2.5rem] flex justify-center items-center rounded bg-purple-950 text-white pointer-events-none cursor-not-allowed"
                href={buildHref(p, search)}
                aria-disabled="true"
              >
                {p}
              </a>
            ) : (
              <a
                key={`${currentPage}-${p}`}
                href={buildHref(p, search)}
                aria-label={`Go to page ${p}`}
                className={`${linkBase} ${linkCursor}`}
                onClick={handleLinkClick(p)}
                {...keyboardNavProps}
                {...readonlyProps}
              >
                {p}
              </a>
            )
          )}
        </div>

        {currentPage < totalPages ? (
          <a
            href={buildHref(currentPage + 1, search)}
            aria-label="Next page"
            title="Next page"
            className={`${linkBase} ${linkCursor}`}
            onClick={handleLinkClick(currentPage + 1)}
            {...keyboardNavProps}
            {...readonlyProps}
          >
            <FaAngleRight aria-hidden="true" />
          </a>
        ) : (
          <div className="min-w-8 sm:min-w-10" />
        )}
      </div>

      <div className="min-w-8 flex justify-end">
        {currentPage !== totalPages && (
          <a
            href={buildHref(totalPages, search)}
            aria-label="Go to last page"
            title="Go to last page"
            className={`${linkBase} ${linkCursor}`}
            onClick={handleLinkClick(totalPages)}
            {...keyboardNavProps}
            {...readonlyProps}
          >
            <FaAnglesRight aria-hidden="true" />
          </a>
        )}
      </div>
    </nav>
  );
}
