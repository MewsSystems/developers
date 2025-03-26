import { Header } from './Header/Header'
import "./Header/header.style.css"
import { SearchBar } from './Content/SearchBar'
import "./Content/SearchBar.style.css"
import { CardContainer } from './Content/CardContainer'
import "./Content/cardContainer.style.css"
import { Button } from './Button/Button'
import "./Button/button.style.css"
import { Footer } from './Footer/Footer'
import "./Footer/footer.style.css"
import { useState } from 'react'
import { useEffect } from 'react'
import { getFormattedMovies } from './constants/getFormattedMovies'
import { getSearchResult } from './constants/getSearchResults'

export const App = () => {

  const [page, setPage] = useState(1)
  const [searchTerm, setSearchTerm] = useState("")
  const [items, setItems] = useState([])

  const handleLoadMore = () => {
      setPage((prev) => prev + 1)
  }

  useEffect(() => {
    if (searchTerm.trim() === "") {

      getFormattedMovies(page).then((data) => {
        setItems((prev) => (page === 1 ? data : [...prev, ...data]));
      });

    } else {

      getSearchResult(searchTerm, page).then((data) => {
        setItems((prev) => (page === 1 ? data : [...prev, ...data]));
      });
    }
  }, [searchTerm, page]);

  useEffect(() => {
    setPage(1);
    setItems([]);
  }, [searchTerm]);

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