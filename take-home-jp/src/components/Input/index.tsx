import React from 'react';
import TextField from '@mui/material/TextField';
import { styles } from './styles';

interface Props {
  onChange?: (text: string) => void;
}

export default function Input({ onChange }: Props) {
  return (
    <TextField
      style={styles.input}
      id="Movie Search"
      label="Search for a movie"
      variant="filled"
      color="primary"
      placeholder="Search for a movie"
      onChange={(event) => onChange?.(event.target.value)}
    />
  );
}
