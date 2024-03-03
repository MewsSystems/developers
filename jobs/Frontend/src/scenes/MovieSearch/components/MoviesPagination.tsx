"use client";

import React from "react";
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination";
import { usePathname, useSearchParams } from "next/navigation";

type MoviesPaginationProps = {
  totalPages: number;
  currentPage: number;
};

const getHref = (
  pathname: string,
  searchParams: URLSearchParams,
  page: number,
) => {
  const urlParams = new URLSearchParams(searchParams.toString());
  urlParams.set("page", String(page));
  return `${pathname}?${urlParams.toString()}`;
};

/**
 * Client-side component used in the MovieSearch scene to display pagination below the results. If there is
 * only one page, the pagination is not displayed. If the user is on the first or last page, the previous
 * and next buttons are disabled respectively.
 *
 * @param totalPages - The total number of pages.
 * @param currentPage - The current page that the user is on.
 */
const MoviesPagination = ({
  totalPages,
  currentPage,
}: MoviesPaginationProps) => {
  const pathname = usePathname();
  const searchParams = useSearchParams();
  if (totalPages === 1) {
    return null;
  }
  return (
    <Pagination className="mt-4">
      <PaginationContent>
        {currentPage > 1 && (
          <PaginationItem>
            <PaginationPrevious
              href={getHref(pathname, searchParams, currentPage - 1)}
            />
          </PaginationItem>
        )}
        <PaginationItem>
          <PaginationLink
            href={getHref(pathname, searchParams, 1)}
            isActive={currentPage === 1}
          >
            1
          </PaginationLink>
        </PaginationItem>
        {currentPage > 2 && (
          <PaginationItem>
            <PaginationEllipsis />
          </PaginationItem>
        )}
        {currentPage > 1 && currentPage < totalPages && (
          <PaginationItem>
            <PaginationLink
              href={getHref(pathname, searchParams, currentPage)}
              isActive
            >
              {currentPage}
            </PaginationLink>
          </PaginationItem>
        )}
        {currentPage < totalPages - 1 && (
          <PaginationItem>
            <PaginationEllipsis />
          </PaginationItem>
        )}
        <PaginationItem>
          <PaginationLink
            href={getHref(pathname, searchParams, totalPages)}
            isActive={currentPage === totalPages}
          >
            {totalPages}
          </PaginationLink>
        </PaginationItem>
        {currentPage < totalPages && (
          <PaginationItem>
            <PaginationNext
              href={getHref(pathname, searchParams, currentPage + 1)}
            />
          </PaginationItem>
        )}
      </PaginationContent>
    </Pagination>
  );
};

export default MoviesPagination;
