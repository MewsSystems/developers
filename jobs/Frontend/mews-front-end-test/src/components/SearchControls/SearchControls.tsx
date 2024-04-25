import React, { FC } from 'react';
import { UseMovies } from '../../hook/useMovies';
import { StyledLegend } from './SearchControl.styled';

interface Props
  extends Pick<
    UseMovies,
    'page' | 'numberOfPages' | 'decrementPageNumber' | 'incrementPageNumber'
  > {
  showControls: boolean;
  id: string;
}

const SearchControls: FC<Props> = ({
  page,
  incrementPageNumber,
  decrementPageNumber,
  numberOfPages,
  showControls,
  id,
}) => {
  return showControls ? (
    <fieldset>
      <StyledLegend>{`navigation-controls-${id}`}</StyledLegend>
      <button onClick={decrementPageNumber}>Back</button>
      {`${page} of ${numberOfPages}`}
      <button onClick={incrementPageNumber}>Next</button>
    </fieldset>
  ) : null;
};

export { SearchControls };
