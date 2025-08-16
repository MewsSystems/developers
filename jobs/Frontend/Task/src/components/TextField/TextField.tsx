import React from "react";
import MuiTextField from "@mui/material/TextField";

type TextFieldProps = {
  onChange: (e: string) => void;
  placeholder: string;
}

export const TextField = ({ placeholder, onChange }: TextFieldProps) => {
  return (
    <MuiTextField
      color="secondary"
      fullWidth
      onChange={(e) => onChange(e.target.value)}
      placeholder={placeholder} 
      variant="outlined"
    />
  );
};
