import { MdHighlightOff } from 'react-icons/md';
import {
  StyledSearchMovieInput,
  StyledSearchMovieInputCleanButton,
  StyledSearchMovieInputContainer,
} from './SearchMovie.styles';

export const SearchMovie = ({
  value,
  onChange,
}: {
  value: string;
  onChange: (value: string) => void;
}) => (
  <StyledSearchMovieInputContainer>
    <StyledSearchMovieInput value={value} onChange={e => onChange(e.target.value)} />
    {value && (
      <StyledSearchMovieInputCleanButton
        onClick={() => onChange('')}
        data-testid="clean-input-search-button"
      >
        <MdHighlightOff />
      </StyledSearchMovieInputCleanButton>
    )}
  </StyledSearchMovieInputContainer>
);
