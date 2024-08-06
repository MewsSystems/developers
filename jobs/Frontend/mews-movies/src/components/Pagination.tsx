import React from "react";
import { PaginationProps } from "../types/PaginationInterfaces";

const Pagination: React.FC<PaginationProps> = ({ page, totalPages, onNextPage, onPrevPage }) => {
  return (
    <div>
      {page > 1 && <button onClick={onPrevPage}>Previous</button>}
      {page < totalPages && <button onClick={onNextPage}>Next</button>}
    </div>
  );
};

export default Pagination;
