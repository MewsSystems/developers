import React, { PropsWithChildren } from "react";
import MuiAppBar from "@mui/material/AppBar";
import { Container } from "@mui/material";

export const AppBar = ({ children }: PropsWithChildren) => {
  return (
    <MuiAppBar elevation={0} position="static">
      <Container>{children}</Container>
    </MuiAppBar>
  );
};
