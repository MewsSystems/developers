import { useDebouncedValue } from '@app/lib/use-debounced-value';
import { useEffect, useState } from 'react';
import { SearchInput } from './search.styled';

interface SearchProps {
  onSearch: (query: string) => void;
  placeholder?: string;
}

export const Search: React.FC<SearchProps> = ({ 
  onSearch, 
  placeholder = 'Search movies...' 
}) => {
  const [value, setValue] = useState('');
  const debouncedValue = useDebouncedValue(value, 500);
  
  useEffect(() => {
    onSearch(debouncedValue);
  }, [debouncedValue, onSearch]);


  return (
      <SearchInput
        type="text"
        value={value}
        onChange={(e) => setValue(e.target.value)}
        placeholder={placeholder}
      />
  );
}; 