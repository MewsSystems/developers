import React from 'react';
import Alert from '@mui/material/Alert';

interface Props {
  errorMessage: string;
}

export default function ErrorLabel({ errorMessage }: Props) {
  return <Alert severity="error">{errorMessage}</Alert>;
}
