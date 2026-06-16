import React from "react";
import Box from "@mui/material/Box";
import MuiDrawer, { DrawerProps as MuiDrawerProps } from "@mui/material/Drawer";

type DrawerProps = {
  anchor: MuiDrawerProps["anchor"];
  children: React.ReactNode;
  isOpen: boolean;
  onClose: () => void;
}

export const Drawer = ({ 
  anchor, 
  children, 
  isOpen, 
  onClose 
}: DrawerProps) => {
  return (
    <MuiDrawer
      anchor={anchor}
      onClose={onClose}
      open={isOpen}
    >
      <Box p={4} height="100%" width="600px">
        {children}
      </Box>
    </MuiDrawer>
  );
};
