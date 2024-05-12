"use client";
import React from "react";
import { useSearchNavigation } from "@/hooks/UseSearchNavigation";

interface MoviePaginationProps {
  totalPages: number;
}

export default function MoviePagination({ totalPages }: MoviePaginationProps) {
  const { movieSearch, searchterm, currentPage } = useSearchNavigation();

  return (
    <>
      <button
        disabled={currentPage === 1}
        onClick={() => movieSearch(searchterm, currentPage - 1)}
      >
        Previous
      </button>
      <button
        disabled={currentPage === totalPages}
        onClick={() => movieSearch(searchterm, currentPage + 1)}
      >
        Next
      </button>
    </>
  );
}
