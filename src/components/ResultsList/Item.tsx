import React, { FC, SyntheticEvent } from 'react';
import {
  Card,
  Poster,
  Info,
  Title,
  Overview,
  StatItem,
  Stats,
  PosterWrapper,
  DetailsLink,
} from './styles';
import { Link } from 'react-router-dom';
import { Movie } from '../../types';

export const Item: FC<{
  result: Movie;
  onNavigation?: (result: Movie) => void;
  toggleInfo?: (id?: number) => void;
  isSelected?: boolean;
}> = ({ result, onNavigation, isSelected, toggleInfo }) => {
  const { id, vote_average, release_date, overview, title, poster_path } = result;

  const handleToggleInfo = (e: SyntheticEvent) => {
    e.preventDefault();
    toggleInfo(isSelected ? undefined : id);
  };

  return (
    <Card key={id}>
      <PosterWrapper>
        <Poster src={`https://image.tmdb.org/t/p/w500/${poster_path}`} alt={title} />
      </PosterWrapper>
      <Info isSelected={isSelected} {...(toggleInfo && { onClick: handleToggleInfo })}>
        <Title>{title}</Title>
        <Overview>{overview}</Overview>
        <DetailsLink>
          <Link to={`/${id}`} onClick={() => onNavigation(result)}>
            See More Details
          </Link>
        </DetailsLink>
        <Stats>
          <StatItem>Release Date: {release_date}</StatItem>
          <StatItem>Rating: {vote_average}</StatItem>
        </Stats>
      </Info>
    </Card>
  );
};
