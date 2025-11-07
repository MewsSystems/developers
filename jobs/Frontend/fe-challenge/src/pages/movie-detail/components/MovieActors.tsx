import { CastPerson } from '@/modules/movies/domain/MovieDetail';
import React from 'react';

interface MovieActorsProps {
  cast: Array<CastPerson>;
}

const MovieActors = ({ cast }: MovieActorsProps) => {
  return cast.map((person, index, cast) => (
    <React.Fragment key={person.originalName}>
      <span>{person.originalName}</span>{' '}
      <span className="font-light text-sm">({person.characterName})</span>
      {`${index !== cast.length - 1 ? ', ' : '.'}`}
    </React.Fragment>
  ));
};

export default MovieActors;
