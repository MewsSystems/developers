import { FC, FormEvent } from 'react';
import { StyledSearchBox } from './SearchBox.styled';
import { UseMovies } from '../../hook/useMovies';

type Props = Pick<UseMovies, 'searchQuery' | 'setSearchQuery'>;

const SearchBox: FC<Props> = ({ searchQuery, setSearchQuery }) => {
  return (
    <form>
      <StyledSearchBox
        placeholder={'Enter a movie name to search'}
        type={'text'}
        value={searchQuery}
        onChange={(event: FormEvent<HTMLInputElement>) => {
          setSearchQuery(event.currentTarget.value);
        }}
      />
    </form>
  );
};

export { SearchBox };
