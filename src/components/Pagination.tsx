'use client';

import { AiFillCaretLeft, AiFillCaretRight } from 'react-icons/ai';
import { HTMLAttributes } from 'react';

interface PaginationProps extends HTMLAttributes<HTMLElement> {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  search?: string;
  readonly?: boolean;
}

const getPageRange = (currentPage: number, totalPages: number): number[] => {
  return Array.from({ length: 5 }, (_, i) => i + Math.max(1, currentPage - 2)).filter(
    (p) => p <= totalPages
  );
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
  ...rest
}: PaginationProps) {
  const linkBase = 'px-2 py-1 rounded transition text-purple-800 hover:underline focus:underline';
  const linkCursor = readonly ? 'cursor-not-allowed' : 'cursor-pointer';

  const readonlyProps = {
    tabIndex: readonly ? -1 : undefined,
    'aria-disabled': readonly ? true : undefined,
  };

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
      className={`flex gap-2 flex-wrap justify-between items-center transition-opacity ${
        readonly ? 'opacity-60 select-none' : ''
      }`}
      {...rest}
    >
      <div className="w-4 flex justify-start">
        {currentPage > 1 && (
          <a
            href={buildHref(currentPage - 1, search)}
            aria-label="Previous page"
            className={`${linkBase} ${linkCursor}`}
            onClick={handleLinkClick(currentPage - 1)}
            {...readonlyProps}
          >
            <AiFillCaretLeft aria-hidden="true" />
          </a>
        )}
      </div>

      <div className="flex gap-1 items-center">
        {pageRange.map((p) =>
          p === currentPage ? (
            <a
              key={`${currentPage}-${p}`}
              aria-current="page"
              tabIndex={-1}
              className="px-2 py-1 rounded bg-purple-950 text-white pointer-events-none cursor-not-allowed"
              href={buildHref(p, search)}
              aria-disabled="true"
            >
              {p}
            </a>
          ) : (
            <a
              key={`${currentPage}-${p}`}
              href={buildHref(p, search)}
              aria-label={`Page ${p}`}
              className={`${linkBase} ${linkCursor}`}
              onClick={handleLinkClick(p)}
              {...readonlyProps}
            >
              {p}
            </a>
          )
        )}

        {!pageRange.includes(totalPages) && (
          <span className="flex gap-2">
            <span aria-hidden="true">…</span>
            <a
              href={buildHref(totalPages, search)}
              className={`${linkBase} ${linkCursor}`}
              aria-label={`Page ${totalPages}`}
              onClick={handleLinkClick(totalPages)}
              {...readonlyProps}
            >
              {totalPages}
            </a>
          </span>
        )}
      </div>

      <div className="w-4 flex justify-end">
        {currentPage < totalPages && (
          <a
            href={buildHref(currentPage + 1, search)}
            aria-label="Next page"
            className={`${linkBase} ${linkCursor}`}
            onClick={handleLinkClick(currentPage + 1)}
            {...readonlyProps}
          >
            <AiFillCaretRight aria-hidden="true" />
          </a>
        )}
      </div>
    </nav>
  );
}
