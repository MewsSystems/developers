import {useSearchParams} from 'react-router-dom';
import {updateUrlParams} from '../utils/updateUrlParams';

export const useSearchInput = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const searchUrlParam = searchParams.get('search') || '';

  const onSearchInputChange = (inputValue: string) => {
    const newParams = updateUrlParams({
      params: searchParams,
      key: 'search',
      value: inputValue || null,
    });

    setSearchParams(newParams);
  };

  return {
    searchUrlParam,
    onSearchInputChange,
    clearSearch: () =>
      setSearchParams(updateUrlParams({params: searchParams, key: 'search', value: null})),
  };
};
