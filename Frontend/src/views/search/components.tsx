import React, { useRef } from "react";
import { ListCollection, ListCollectionItem } from "../../components/collection";
import { MovieResult } from "../../services/types";
import { StyledLink, StyledInput, StyledLabel, Button } from "../../components";

export type MovieListProps = {
  movies: MovieResult[],
}

export const MovieList: React.FC<MovieListProps> = ({ movies }) => {
  return (
    <ListCollection>
      {movies.map(m => (
        <ListCollectionItem key={m.id}>
          <StyledLink to={`/movie/${m.id}`}>
            {m.title}
          </StyledLink>
        </ListCollectionItem>
      ))}
    </ListCollection>
  )
}

type SearchBoxProps = {
  isBusy: boolean,
  onSearch: (query: string) => void,
};

export const SearchBox: React.FC<SearchBoxProps> = ({ isBusy, onSearch }) => {
  const searchInput = useRef<HTMLInputElement>(null);

  const onClick = () => {
    if (searchInput.current && searchInput.current.value.trim().length > 0) {
      onSearch(searchInput.current.value.trim());
    }
  }

  return (
    <div style={{ display: 'flex', position: 'relative', alignItems: 'center' }}>
      <StyledInput type="text" placeholder="Godfather" ref={searchInput} />
      <StyledLabel>Search...</StyledLabel>
      <Button disabled={isBusy} onClick={onClick} style={{ marginLeft: '10px' }} >Search</Button>
    </div>
  )
}

