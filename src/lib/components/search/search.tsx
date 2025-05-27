import { useDebouncedValue } from '@app/lib/use-debounced-value';
import { useEffect, useState } from 'react';
import { SearchInput } from './search.styled';

interface SearchProps {
  onSearch: (query: string) => void;
  placeholder?: string;
}

export const Search = ({ onSearch, placeholder = 'Search movies...' }: SearchProps) => {
  const [value, setValue] = useState('');
  const debouncedValue = useDebouncedValue(value);

  useEffect(() => {
    onSearch(debouncedValue);
  }, [debouncedValue, onSearch]);

  return (
    <SearchInput
      type="text"
      value={value}
      onChange={(e) => setValue(e.target.value)}
      placeholder={placeholder}
      aria-label="Search movies"
    />
  );
};
