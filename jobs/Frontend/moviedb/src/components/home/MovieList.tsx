import React from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import { RootState } from '../../redux/store';
import ListItem from './MovieListItem';

const List = styled.ul`
    margin-bottom: 60px;
`;

const InfoText = styled.p`
  font-size: 24px;
  text-align: center;
`;

export default function MovieList() {
  const { searchQuery, data } = useSelector((state: RootState) => state.search);

  return (
    <>
      {data && data.results.length > 0
        && (
        <List>
          {data.results.map((item: IFetchMovieItem) => (
            <ListItem key={item.id} item={item} />
          ))}
        </List>
        )}

      {data && !data.results.length
        && <InfoText>{`No results for search term "${searchQuery}".`}</InfoText>}

      {!data
        && <InfoText>Enter search term.</InfoText>}
    </>
  );
}
