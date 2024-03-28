import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import { Link } from "react-router-dom";

export const BackToHomeButton = () => {
  return (
    <Box mt={2}>
      <Link to="/">
        <Button>Search new movie</Button>
      </Link>
    </Box>
  );
};
