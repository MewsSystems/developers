import { Card, CardContent, CardMedia, Typography } from "@mui/material";
import { FC } from "react";
import { PartialMovie } from "../../../redux/movies/movies.slice.types";
import { getImageURL } from "../../../utils/image";
import { StyledCardLink } from "./StyledCardLink";

export const MovieCard: FC<{ movie: PartialMovie }> = ({ movie }) => {
  return (
    <StyledCardLink to={`/movie/${movie.id}`}>
      <Card elevation={0}>
        <CardMedia
          component="img"
          height="140"
          image={getImageURL(movie.poster_path, "w200")}
          alt={movie.title}
          sx={{ objectFit: "contain" }}
        />
        <CardContent>
          <Typography gutterBottom variant="subtitle1">
            {movie.title}
          </Typography>
          <Typography variant="caption" color="text.secondary">
            release date: {movie.release_date}
          </Typography>
        </CardContent>
      </Card>
    </StyledCardLink>
  );
};
