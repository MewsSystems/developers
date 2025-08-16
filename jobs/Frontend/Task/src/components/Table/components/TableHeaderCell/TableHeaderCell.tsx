import React from "react";
import Box from "@mui/material/Box";
import TableCell from "@mui/material/TableCell";
import Typography from "@mui/material/Typography";

type TableHeaderCellProps = {
  label: string;
}

export const TableHeaderCell = ({ label }: TableHeaderCellProps) => {
  return (
    <TableCell>
      <Typography variant='h6'>
        <Box fontWeight="400">
          {label}
        </Box>
      </Typography>
    </TableCell>
  );
};
