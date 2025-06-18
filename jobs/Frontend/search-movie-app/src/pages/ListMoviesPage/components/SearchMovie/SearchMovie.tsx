import { MdHighlightOff } from 'react-icons/md';
import {
  StyledSearchMovieInput,
  StyledSearchMovieInputCleanButton,
  StyledSearchMovieInputContainer,
} from './SearchMovie.styles';
import { CLEAN_INPUT_SEARCH_BUTTON_TEST_ID } from '../../../../constants';

export const SearchMovie = ({
  value,
  onChange,
}: {
  value: string;
  onChange: (value: string) => void;
}) => (
  <StyledSearchMovieInputContainer>
    <StyledSearchMovieInput
      value={value}
      onChange={e => onChange(e.target.value)}
      placeholder="Type to start searching..."
    />
    {value && (
      <StyledSearchMovieInputCleanButton
        onClick={() => onChange('')}
        data-testid={CLEAN_INPUT_SEARCH_BUTTON_TEST_ID}
      >
        <MdHighlightOff />
      </StyledSearchMovieInputCleanButton>
    )}
  </StyledSearchMovieInputContainer>
);
