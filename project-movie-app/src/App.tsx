import { useState } from 'react'
import { useEffect } from 'react'
import { getPopularMovies } from './API/getPopularMovies'
import { getSearchedMovies } from './API/getSearchedMovies'
import { MovieCard } from './types/movie'
import { HomePage } from './HomePage'

export const App = () => {

  const [page, setPage] = useState<number>(1)
  const [searchTerm, setSearchTerm] = useState<string>("")
  const [items, setItems] = useState<MovieCard[]>([])
  const [debouncedSearchTerm, setDebouncedSearchTerm] = useState<string>("")

  const handleLoadMore = (): void  => {
      setPage((prev) => prev + 1)
  }

  useEffect(() => {
    const timeout = setTimeout(() => {
      setDebouncedSearchTerm(searchTerm);
    }, 500); // waits 500ms
  
    return () => clearTimeout(timeout); // clear if typing continues
  }, [searchTerm]);

  useEffect(() => {
    if (debouncedSearchTerm.trim() === "") {

      getPopularMovies(page).then((data) => {
        setItems((prev) => (page === 1 ? data : [...prev, ...data]));
      });

    } else {

      getSearchedMovies(debouncedSearchTerm, page).then((data) => {
        setItems((prev) => (page === 1 ? data : [...prev, ...data]));
      });
    }
  }, [debouncedSearchTerm, page]);

  useEffect(() => {
    setPage(1);
    setItems([]);
  }, [debouncedSearchTerm]);

  return (
      <HomePage searchTerm={searchTerm} setSearchTerm={setSearchTerm} page={page} items={items} handleLoadMore={handleLoadMore}/>
  )
}