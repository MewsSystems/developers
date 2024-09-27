import React, { FC } from 'react';
import { UseMovies } from '../../hook/useMovies';
import {
  StyledButton,
  StyledFieldSet,
  StyledLegend,
} from './SearchControl.styled';

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
    <StyledFieldSet>
      <StyledLegend>{`navigation-controls-${id}`}</StyledLegend>
      <StyledButton onClick={decrementPageNumber}>Back</StyledButton>
      {`${page} of ${numberOfPages}`}
      <StyledButton onClick={incrementPageNumber}>Next</StyledButton>
    </StyledFieldSet>
  ) : null;
};

export { SearchControls };
