import React, { ReactNode } from 'react';
import Box from '@mui/material/Box';
import { styles } from './styles';

interface Props {
  children: ReactNode;
}

export default function Banner({ children }: Props) {
  return (
    <Box my={4} alignItems="center" px={5} py={10} sx={styles.container}>
      {children}
    </Box>
  );
}
