import React from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { createImgPath, getReleaseYear } from '../../utils';
import { IconsMovie } from '../icons';

const ListItem = styled.li`
  display: flex;
  align-items: center;
  margin-bottom: 16px;
  
  > a {
    flex-shrink: 0;
    margin-right: 30px;
  }
 
  strong {
    display: block;
  }
  
  p {
    font-size: 18px;
    margin-bottom: 4px;
  }
  
  @media (max-width: 539px) {
    > a {
      margin-right: 20px;
      width: 30%;
    }
  }
`;

const ImgPlaceholder = styled.span`
    aspect-ratio: 2 / 3;
    background: #eee;
    display: flex;
    align-items: center;
    justify-content: center;
    max-width: 100%;
    width: 200px;
`;

const MovieTitle = styled(Link)`
  color: #000;
  font-size: 32px;
  font-weight: 700;
  margin-bottom: 16px;
  text-decoration: none;
  text-decoration-color: #fff;
  transition: all .2s;
    
  &:hover,
  &:active,
  &:focus {
    text-decoration: underline;
    text-decoration-color: #999;
  }
  
  @media (max-width: 767px) {
    font-size: 24px;
  }
  
  @media (max-width: 419px) {
    font-size: 20px;
  }
`;

interface IMovieListItemProps {
  item: IFetchMovieItem
}

export default function MovieListItem({ item }: IMovieListItemProps) {
  return (
    <ListItem key={item.id}>
      <Link to={`/movie/${item.id}`}>
        {item.poster_path
          ? (
            <img
              src={createImgPath(item.poster_path, 200)}
              alt={item.title}
              width={200}
              height={300}
            />
          )
          : (
            <ImgPlaceholder>
              <IconsMovie color="#ccc" width="56px" height="56px" />
            </ImgPlaceholder>
          )}
      </Link>
      <div>
        <MovieTitle to={`/movie/${item.id}`}>
          {item.title}
        </MovieTitle>

        {item.original_title !== item.title
           && <p>{item.original_title}</p>}
        <p>
          {item.release_date ? getReleaseYear(item.release_date) : 'TBA'}
          {`, ${item.original_language.toUpperCase()}`}
        </p>
        {item.vote_count > 0
          ? (
            <p>
              {`${item.vote_average} / 10`}
            </p>
          )
          : <p>No votes yet</p>}
      </div>
    </ListItem>
  );
}
