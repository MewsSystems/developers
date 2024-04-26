import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { searchMovie } from '../../api/searchMovie';

export default function SearchPage() {
  const { t } = useTranslation();

  useEffect(() => {
    document.title = t('common.appTitle');
  }, [t]);

  const [query, setQuery] = useState<string>('');

  const { isLoading, data, error } = useQuery({
    queryKey: ['movies', query],
    queryFn: async () =>
      searchMovie(query)
        .then(response => response.data)
        .catch((error: AxiosError) => {
          console.error(error.toJSON());
          throw new Error(`Failed to fetch movies`);
        })
  });

  let content;

  if (isLoading) {
    content = <p>Loading...</p>;
  } else if (error) {
    content = <p>Error: {error.message}</p>;
  } else {
    content = (
      <ul>
        {data?.map(movie => (
          <li key={movie.id}>
            {`${movie.title} (${movie.release_date} | R-18: ${movie.adult} | Popularity: ${movie.popularity}`}
          </li>
        ))}
      </ul>
    );
  }

  return content;
}
