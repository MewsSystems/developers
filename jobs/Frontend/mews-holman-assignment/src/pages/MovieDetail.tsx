import { Link, useParams } from "react-router-dom";
import useGetMovieDetail from "../customHooks/useGetMovieDetail";
import {
  Box,
  Typography,
  CircularProgress,
  Paper,
  Button,
  Rating,
} from "@mui/material";

const MovieDetail = () => {
  const { id } = useParams<{ id: string }>();
  const {
    data: movieData,
    loading,
    error,
  } = useGetMovieDetail(id || "");

  if (loading)
    return (
      <Box
        sx={{
          width: "100%",
          marginTop: "5rem",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <CircularProgress />
      </Box>
    );
  if (error)
    return (
      <Typography variant="h6">Error: {error}</Typography>
    );

  if (!movieData)
    return (
      <Typography variant="h6">
        No movieData found
      </Typography>
    );

  const imageUrl = movieData.poster_path
    ? `https://image.tmdb.org/t/p/w500${movieData.poster_path}`
    : "https://www.svgrepo.com/show/508699/landscape-placeholder.svg";

  const movieGenres = movieData.genres
    .map((genre) => genre.name)
    .join(", ");

  return (
    <Paper
      sx={{
        display: "flex",
        gap: "2rem",
        padding: "2rem",
        "@media (max-width: 600px)": {
          flexDirection: "column",
        },
      }}
    >
      <Box sx={{ flex: "0 50%" }}>
        <img
          src={imageUrl}
          alt={movieData.title}
          style={{ width: "100%", height: "auto" }}
        />
      </Box>
      <Box
        sx={{
          flex: "0 50%",
          display: "flex",
          flexDirection: "column",
          gap: ".75rem",
        }}
      >
        <Typography sx={{ fontSize: "2rem" }}>
          {movieData.title}
        </Typography>
        <Typography>
          {movieData.overview || "No description provided"}
        </Typography>
        <Typography variant="subtitle1">
          Release Date: {movieData.release_date}
        </Typography>
        <Typography variant="subtitle1">
          Genres: {movieGenres}
        </Typography>
        <Box sx={{ display: "flex", gap: ".5rem" }}>
          <Rating
            readOnly
            name="half-rating"
            defaultValue={movieData.vote_average / 2}
            max={5}
            precision={0.1}
          />
          <Typography sx={{ opacity: ".75" }}>
            {movieData.vote_count} votes
          </Typography>
        </Box>
        {movieData.homepage && (
          <a href={movieData.homepage} target="_blank">
            Visit Homepage of the movie
          </a>
        )}
        <Box sx={{ marginTop: "2rem" }}>
          <Button
            component={Link}
            to="/"
            variant="contained"
          >
            Go Back
          </Button>
        </Box>
      </Box>
    </Paper>
  );
};

export default MovieDetail;
