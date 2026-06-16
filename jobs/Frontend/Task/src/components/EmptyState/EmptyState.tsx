import React from "react";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";

type EmptyStateProps = {
  title: string;
  description: string;
}

export const EmptyState = ({ title, description }: EmptyStateProps) => {
  return (
    <Box 
      alignItems="center" 
      display="flex"
      justifyContent="center"
      height="100%"
      flexDirection="column"
    >
      <Typography gutterBottom variant="h5">
        <Box fontWeight={600}>
          {title}
        </Box>
      </Typography> 
      <Typography>
        {description}
      </Typography>
    </Box>
  );
};
