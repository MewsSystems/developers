import { useEffect, useState } from 'react';
import { useDebounce } from 'usehooks-ts';
import { setSearchMovieAction } from '@/store';
import { useDispatch } from 'react-redux';
import Flex from '@/components/Flex';

const SearchInput = () => {
  const dispatch = useDispatch()
  const [currentSearch, setCurrentSearch] = useState<string | null>(null);
  const debouncedValue = useDebounce(currentSearch, 250);

  useEffect(() => {
    if(debouncedValue) dispatch(setSearchMovieAction(debouncedValue))
  }, [debouncedValue]);

  return (
    <Flex>
      <span>Search Movie:</span>
      <input data-testid="search-input" type="search" value={currentSearch ?? ''} onChange={(event) => setCurrentSearch(event.currentTarget.value)}/>
    </Flex>
  )
}

export default SearchInput
