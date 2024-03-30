import Card from "@mui/material/Card";
import { useAppSelector } from "../../../redux/hooks";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";
import { CardMedia, CardContent, Typography, Grid } from "@mui/material";
import { getImageURL } from "../../../utils/image";
import { getFormattedPrice } from "../../../utils/price";

export const MovieDetailsCard = () => {
  const movie = useAppSelector(moviesSelectors.getSelectedMovie);

  if (!movie) {
    return null;
  }

  // return a card component showing many interesting details about the movie
  return (
    <Card elevation={0}>
      <CardMedia
        component="img"
        height="200"
        image={getImageURL(movie.poster_path, "w300")}
        alt={movie.title}
        sx={{ objectFit: "contain" }}
      />
      <CardContent>
        <Grid container spacing={1}>
          <Grid item xs={12}>
            <Typography
              variant="body1"
              color="text.secondary"
              textAlign="justify"
            >
              sinopsis: <b>{movie.overview}</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              release date: <b>{movie.release_date}</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              duration: <b>{movie.runtime} minutes</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              status: <b>{movie.status}</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              budget: <b>{getFormattedPrice(movie.budget)}</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              revenue: <b>{getFormattedPrice(movie.revenue)}</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              vote average: <b>{movie.vote_average.toFixed(1)}</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              vote count: <b>{movie.vote_count}</b>
            </Typography>
          </Grid>
          <Grid item xs={6} sm={4}>
            <Typography variant="body2" color="text.secondary">
              genres:{" "}
              <b>{movie.genres.map((genre) => genre.name).join(", ")}</b>
            </Typography>
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  );
};
