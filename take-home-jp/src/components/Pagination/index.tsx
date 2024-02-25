import React from 'react';
import Box from '@mui/material/Box';
import { Pagination as BasePagination } from '@mui/material';
import { styles } from './styles';

interface Props {
  currentPage: number;
  totalPages: number;
  onChange: (page: number) => void;
}

export default function Pagination({
  onChange,
  currentPage,
  totalPages,
}: Props) {
  return (
    <Box py={2} style={styles.container}>
      <BasePagination
        onChange={(_: unknown, page: number) => onChange(page)}
        page={currentPage}
        count={totalPages}
        size="large"
        shape="rounded"
        sx={styles.buttonItems}
      />
    </Box>
  );
}
