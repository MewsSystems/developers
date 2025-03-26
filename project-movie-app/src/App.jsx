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
        return
    }
    getSearchResult(searchTerm).then((data) => {
      setItems(data)
    })
  },[searchTerm])

  
  useEffect(()=> {
    if (searchTerm.trim() !== "") {
      return
    }
    getFormattedMovies(page).then((data) => {
        if (page === 1) {
            setItems(data) // first load
        }
        else {
            setItems((prev) => 
                [...prev, ...data]); // pagination
        }
    })
  },[page])

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