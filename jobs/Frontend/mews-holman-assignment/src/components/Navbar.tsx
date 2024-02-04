import Typography from "@mui/material/Typography";
import { Box } from "@mui/material";

const Navbar = () => {
  return (
    <Box
      position="static"
      sx={{
        padding: "1rem",
        display: "flex",
        justifyContent: "space-between",
        backgroundColor: "primary.main",
      }}
    >
      <Typography
        variant="h1"
        component="div"
        sx={{ fontWeight: "bold", fontSize: "2rem" }}
        color={"white"}
      >
        Movie searcher
      </Typography>
    </Box>
  );
};

export default Navbar;
