import React from "react";
import { Movie } from "../types/types";
import {
  Card,
  CardContent,
  Typography,
  CardMedia,
  Button,
} from "@mui/material";
import { useNavigate } from "react-router";

type MovieCardProps = {
  movieData: Movie;
};

const MovieCard: React.FC<MovieCardProps> = ({
  movieData,
}) => {
  const navigate = useNavigate();

  const handleButtonClick = () => {
    navigate(`/movie-detail/${movieData.id}`);
  };

  const imageUrl = movieData.poster_path
    ? `https://image.tmdb.org/t/p/w500${movieData.poster_path}`
    : "https://www.svgrepo.com/show/508699/landscape-placeholder.svg";

  return (
    <Card sx={{ width: "20rem" }}>
      <CardMedia
        component="img"
        height="140"
        image={imageUrl}
        alt={movieData.title}
      />
      <CardContent>
        <Typography
          gutterBottom
          variant="h5"
          component="div"
        >
          {movieData.title}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Release date: {movieData.release_date.slice(0, 4)}
        </Typography>
        <Button
          variant="contained"
          onClick={handleButtonClick}
          sx={{ mt: 3 }}
        >
          More Info
        </Button>
      </CardContent>
    </Card>
  );
};

export default MovieCard;
