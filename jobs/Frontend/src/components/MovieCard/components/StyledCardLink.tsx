import { styled } from "@mui/material";
import { Link } from "react-router-dom";

export const StyledCardLink = styled(Link)(({ theme }) => ({
  display: "flex",
  width: "100%",
  height: "100%",
  textDecoration: "none",
  color: "inherit",
  border: `2px solid ${theme.palette.divider}`,
  borderRadius: theme.shape.borderRadius,
  "&:hover": {
    textDecoration: "none",
    color: "inherit",
    borderColor: theme.palette.primary.main,
  },
  "& > :first-of-type": {
    flex: "1 1 auto",
  },
}));
