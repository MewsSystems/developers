import { StyledSearchMovieInput } from './SearchMovie.styles';

export const SearchMovie = ({
  value,
  onChange,
}: {
  value: string;
  onChange: (value: string) => void;
}) => <StyledSearchMovieInput value={value} onChange={e => onChange(e.target.value)} />;
