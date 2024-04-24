import React, { FC } from 'react';
import { UseMovies } from '../../hook/useMovies';

interface Props
  extends Pick<
    UseMovies,
    'page' | 'numberOfPages' | 'decrementPageNumber' | 'incrementPageNumber'
  > {
  showControls: boolean;
}

const SearchControls: FC<Props> = ({
  page,
  incrementPageNumber,
  decrementPageNumber,
  numberOfPages,
  showControls,
}) => {
  return showControls ? (
    <div>
      <button onClick={decrementPageNumber}>Back</button>
      {`${page} of ${numberOfPages}`}
      <button onClick={incrementPageNumber}>Next</button>
    </div>
  ) : null;
};

export { SearchControls };
