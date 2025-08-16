import React from "react";
import MuiTable from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import { TableHeaderCell } from "./components/TableHeaderCell/TableHeaderCell";

type Column<T> = {
  key: Extract<keyof T, string>;
  label: string;
  render: (a: T) => React.JSX.Element;
}

type TableProps<T> = {
  columns: Column<T>[];
  data: T[];
  onClickRow?: (a: T) => void;
}

export const Table = <T,>({ columns, data, onClickRow }: TableProps<T>) => {  
  return (
    <TableContainer>
      <MuiTable size="small">
        <TableHead>
          <TableRow>
            {columns.map(({ key, label }) => (
              <TableHeaderCell key={key} label={label} />
            ))}
          </TableRow>
        </TableHead>
        
        <TableBody>
          {data.map((row, i) => (
            <TableRow 
              key={i} 
              onClick={onClickRow ? () => onClickRow(row) : undefined}
              sx={{ 
                cursor: onClickRow ? "pointer" : "default",
                "&:hover": { 
                  backgroundColor: onClickRow ? "#E9EDEF" : "default"
                }
              }}
            >
              {columns.map(column => column.render(row))}
            </TableRow>
          ))}
        </TableBody>
      </MuiTable>
    </TableContainer>
  );
};
