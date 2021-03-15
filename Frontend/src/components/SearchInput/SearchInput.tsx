import { Search, TimesCircle } from '@styled-icons/fa-solid';
import { useCallback, useEffect, useRef } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';
import { UrlUpdateType } from 'use-query-params';
import {
  InputContainer,
  Input,
  InputContainerProps,
  InputClearButton,
  InputIconContainer,
} from './styled';
import { useMovieSearch, useSearchQueryParams } from '../../hooks';

interface SearchInputProps extends InputContainerProps {
  placeholderText?: string;
  resultsPath: string;
}

const SearchInput = ({
  maxWidth,
  resultsPath,
  placeholderText = 'Search...',
}: SearchInputProps) => {
  const searchMovies = useMovieSearch();
  const inputRef = useRef<HTMLInputElement>(null);
  const focusInput = () => inputRef.current && inputRef.current.focus();
  const history = useHistory();
  const match = useRouteMatch({ path: resultsPath, exact: true });
  const [{ query, page }, setQueryParams] = useSearchQueryParams();

  const setSearchQuery = useCallback(
    (value) => {
      let updateType: UrlUpdateType = 'pushIn';

      // redirect to results page
      if (!match) {
        history.push(resultsPath);
        updateType = 'replaceIn';
      }

      setQueryParams(
        { query: value !== '' ? value : undefined, page: undefined },
        updateType
      );
    },
    [history, resultsPath, setQueryParams, match]
  );

  // call API whenever url search params change
  useEffect(() => {
    if (match?.isExact) {
      searchMovies(query || '', page || undefined);
    }
  }, [query, page, searchMovies, match?.isExact]);

  return (
    <InputContainer maxWidth={maxWidth}>
      <InputIconContainer>
        <Search size="1.5rem" onClick={focusInput} />
      </InputIconContainer>
      <Input
        ref={inputRef}
        type="text"
        value={query || ''}
        onChange={(e) => setSearchQuery(e.target.value)}
        placeholder={placeholderText}
      />
      {query && (
        <InputClearButton type="button" onClick={() => setSearchQuery('')}>
          <TimesCircle size="1.5rem" />
        </InputClearButton>
      )}
    </InputContainer>
  );
};

export default SearchInput;
