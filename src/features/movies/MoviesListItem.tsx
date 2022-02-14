import {Button, Card, CardActions, CardActionArea, CardContent, CardMedia, Typography} from '@mui/material';
import React from 'react';
import {Link} from 'react-router-dom';
import {MovieItem} from './moviesSlice';
import noImageAvailable from '../../no_image_available.png';
import styles from './Movies.module.css';
import {useTranslation} from 'react-i18next';

interface MoviesListItemProps {
  item: MovieItem;
}

export function MoviesListItem(props: MoviesListItemProps) {
  const {item} = props;
  const {t} = useTranslation();

  return (
    <Card elevation={0} className={styles.movieItem}>
      <CardActionArea component={Link} to={`/movie/${item.id}`}>
        <CardMedia
          component="img"
          alt={item.title}
          height="300"
          image={item.poster_path ? `https://image.tmdb.org/t/p/w400${item.poster_path}` : noImageAvailable}
        />
      </CardActionArea>
      <CardContent sx={{minHeight: '72px'}}>
        <Typography gutterBottom variant="h5" component="div">
          {item.title}
        </Typography>
      </CardContent>
      <CardActions>
        <Button
          component={Link}
          to={`/movie/${item.id}`}
          size="small"
          color="primary"
          variant="contained"
          fullWidth
          sx={{boxShadow: 0}}
        >
          {t('movie.detail.button')}
        </Button>
      </CardActions>
    </Card>
  );
}
