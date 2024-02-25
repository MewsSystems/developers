import React from 'react';
import Card from '@mui/material/Card';
import Link from '@mui/material/Link';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Typography from '@mui/material/Typography';
import { Movie } from './types';
import { styles } from './styles';
import { colors } from '../../styling/colors';
import { getThumbnailUrl } from './utils';

interface Props {
  movie: Movie;
}

export default function MovieCard({ movie }: Props) {
  const { id, overview, title, poster_path } = movie;

  return (
    <Card style={styles.cardContainer}>
      <CardMedia
        sx={{ height: 250 }}
        image={getThumbnailUrl(poster_path)}
        title={title}
      />
      <CardContent
        style={{
          padding: '15px',
          flex: 1,
        }}
      >
        <Typography gutterBottom>
          <Link
            variant="h5"
            href={`/movieDetail/${id}`}
            style={styles.cardLink}
          >
            {title}
          </Link>
        </Typography>
        <Typography variant="body2" color={colors.silver}>
          {`${overview.slice(0, 150)}...`}
        </Typography>
      </CardContent>
    </Card>
  );
}
