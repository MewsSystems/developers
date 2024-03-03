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
