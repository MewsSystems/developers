import MovieImg from "./MovieImg.tsx";
import { Box, Button, Rating, Typography } from "@mui/material";
import { Movie } from "../hooks/movies/useSearchMovie.ts";

export const MovieTile = ({ ...props }: Movie) => {
  const {
    overview,
    poster_path: posterPath,
    title,
    vote_average: voteAverage,
    vote_count: voteCount,
  } = props;

  return (
    <Box
      sx={{
        color: "#F9F8FF",
        borderRadius: 5,
        backgroundColor: "#202238",
        p: 1,
        display: "flex",
        flexWrap: { xs: "wrap", md: "nowrap" },
        justifyContent: { xs: "center", md: "flex-start" },
        alignItems: "center",
        gap: 2,
        overflow: "hidden",
        textAlign: { xs: "center", md: "left" },
      }}
    >
      <MovieImg title={title} posterPath={posterPath} />
      <div>
        <Typography variant="h3" sx={{ mt: 1 }}>
          {title}
        </Typography>
        <Box
          sx={{
            display: "flex",
            gap: 1,
            alignItems: "center",
            justifyContent: { xs: "center", md: "flex-start" },
          }}
        >
          <Rating
            max={5}
            precision={0.5}
            name="Movie rating"
            size="small"
            value={voteAverage / 2}
            readOnly
          />
          <Typography variant="body2" sx={{ fontSize: 12 }}>
            ({voteCount})
          </Typography>
        </Box>
        <Typography
          variant="body2"
          sx={{
            mt: 1,
            mb: 2,
            display: "-webkit-box",
            maxWidth: "100%",
            WebkitLineClamp: 3,
            WebkitBoxOrient: "vertical",
            overflow: "hidden",
            textOverflow: "ellipsis",
          }}
        >
          {overview}
        </Typography>
        <Button
          variant="contained"
          sx={{
            width: { xs: 1, md: "auto" },
            textTransform: "initial",
          }}
        >
          Go to detail
        </Button>
      </div>
    </Box>
  );
};

export default MovieTile;
