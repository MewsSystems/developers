import { Header } from './Header'
import { SearchBar } from "./Content/SearchBar"
import { CardContainer } from './Content/CardContainer'
import { Button } from './Button'
import { Footer } from "./Footer"
import { useState } from 'react'
import { useEffect } from 'react'
import { getPopularMovies } from './API/getPopularMovies'
import { getSearchedMovies } from './API/getSearchedMovies'
import { MovieCard } from './types/movie'

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
    <>
      <Header/>
        <main>
          <div className="app__container">
            <h2>Popular Movies</h2>
            <SearchBar searchTerm={searchTerm} setSearchTerm={setSearchTerm}/>
            <CardContainer page={page} items={items}/>
            <Button label="Show more" handleLoadMore={handleLoadMore}/>
          </div>
        </main>
      <Footer/>
    </>
  )
}