import React, { SyntheticEvent } from 'react';
import { Link } from 'react-router-dom';
import {
  Card,
  PosterWrapper,
  Poster,
  Info,
  Title,
  Overview,
  StatItem,
  DetailsLink,
  Stats,
  NoImage,
} from './styles';
import { ItemType } from './Item';

export const MovieItem: ItemType = ({ result, onNavigation, isSelected, toggleInfo }) => {
  const { id, vote_average, release_date, overview, title, poster_path } = result;

  const handleToggleInfo = (e: SyntheticEvent) => {
    e.preventDefault();
    toggleInfo?.(isSelected ? undefined : id);
  };

  return (
    <Card key={id}>
      <PosterWrapper>
        {poster_path ? (
          <Poster src={`https://image.tmdb.org/t/p/w500/${poster_path}`} alt={title} />
        ) : (
          <NoImage>📽️</NoImage>
        )}
      </PosterWrapper>
      <Info isSelected={isSelected} {...(toggleInfo && { onClick: handleToggleInfo })}>
        <Title>{title}</Title>
        <Overview>{overview}</Overview>

        <Stats>
          <StatItem>Release Date: {release_date}</StatItem>
          <StatItem>Rating: {vote_average}</StatItem>
        </Stats>
        <DetailsLink>
          <Link to={`/${id}`} onClick={() => onNavigation?.(result)}>
            See More Details
          </Link>
        </DetailsLink>
      </Info>
    </Card>
  );
};
