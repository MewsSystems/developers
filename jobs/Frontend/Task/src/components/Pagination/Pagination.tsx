import React from "react";
import Box from "@mui/material/Box";
import MuiPagination from "@mui/material/Pagination";

type PaginationProps = {
  count?: number;
  onChange: (page: number) => void;
  page?: number;
}

export const Pagination = ({ count, onChange, page }: PaginationProps) => {
  if (!count || count === 1) {
    return null;
  }
  
  return (
    <Box display="flex" justifyContent="center">
      <MuiPagination 
        count={count} 
        onChange={(event, page) => onChange(page)}
        page={page} 
      />
    </Box>
  );
};
