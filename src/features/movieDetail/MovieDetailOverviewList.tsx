import {Grid, Typography} from '@mui/material';
import React from 'react';
import {MovieGenre, MovieProductionCompany, MovieProductionCountries} from './movieDetailSlice';

import styles from './MovieDetail.module.css';

interface MovieDetailOverviewListProps {
  items: MovieGenre[] | MovieProductionCompany[] | MovieProductionCountries[];
  title: string;
}

export function MovieDetailOverviewList(props: MovieDetailOverviewListProps) {
  const {items, title} = props;

  const parseNames = (names: MovieGenre[] | MovieProductionCompany[] | MovieProductionCountries[]) => {
    let parsedNames = '';
    names.forEach((name: MovieGenre | MovieProductionCompany | MovieProductionCountries, index: number) => {
      parsedNames += name.name;
      if (index < names.length - 1) {
        parsedNames += ', ';
      }
    });
    return parsedNames;
  };

  return (
    <Grid item container direction="row">
      <Grid item xs={3}>
        <Typography variant="body1" component="div" className={styles.movieOverviewListTitle}>
          {title}
        </Typography>
      </Grid>
      <Grid item>
        <Typography variant="body1" component="div">
          {parseNames(items)}
        </Typography>
      </Grid>
    </Grid>
  );
}
