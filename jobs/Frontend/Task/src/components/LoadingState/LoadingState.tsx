import React from "react";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";

export const LoadingState = () => {
  return (
    <Box 
      alignItems='center' 
      display='flex' 
      justifyContent='center'
      height="100%"
    >
      <CircularProgress />
    </Box>
  );
};
