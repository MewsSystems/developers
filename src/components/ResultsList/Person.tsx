import React, { SyntheticEvent } from 'react';
import { Link } from 'react-router-dom';
import { ItemType } from './Item';
import {
  Card,
  PosterWrapper,
  Poster,
  NoImage,
  Info,
  Title,
  List,
  ListItem,
  DetailsLink,
} from './styles';

export const PersonItem: ItemType = ({ result, onNavigation, isSelected, toggleInfo }) => {
  const { id, name, known_for, gender, title, profile_path } = result;

  const handleToggleInfo = (e: SyntheticEvent) => {
    e.preventDefault();
    toggleInfo?.(isSelected ? undefined : id);
  };

  return (
    <Card key={id}>
      <PosterWrapper>
        {profile_path ? (
          <Poster src={`https://image.tmdb.org/t/p/w500/${profile_path}`} alt={title} />
        ) : (
          <NoImage>{gender === 0 ? <span>🕺🏼</span> : <span>💃🏼</span>}</NoImage>
        )}
      </PosterWrapper>
      <Info isSelected={isSelected} {...(toggleInfo && { onClick: handleToggleInfo })}>
        <Title>{name}</Title>

        <List>
          <h3>Known For:</h3>
          {known_for?.map((item) => {
            return <ListItem key={item.id}>{item.title}</ListItem>;
          })}
        </List>
        <DetailsLink>
          <Link to={`/${id}`} onClick={() => onNavigation?.(result)}>
            See More Details
          </Link>
        </DetailsLink>
      </Info>
    </Card>
  );
};
