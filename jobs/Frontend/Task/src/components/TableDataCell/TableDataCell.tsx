import React from "react";
import TableCell from "@mui/material/TableCell";
import Typography from "@mui/material/Typography";
import { NO_VALUE_PLACEHOLDER } from "../../constants/index";

type TableDataCellProps = {
  label: string | undefined;
  maxWidth?: number | string;
}

export const TableDataCell = ({ label, maxWidth = "none"}: TableDataCellProps) => {
  return (
    <TableCell sx={{ maxWidth }}>
      <Typography noWrap>{label || NO_VALUE_PLACEHOLDER}</Typography>
    </TableCell>
  );
};
