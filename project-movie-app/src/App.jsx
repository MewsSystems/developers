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

export const App = () => {

  const [page, setPage] = useState(1)

  const handleLoadMore = () => {
      setPage((prev) => prev + 1)
  }

  return (
    <>
      <Header/>
        <main>
          <div className="app__container">
            <h2>Popular Movies</h2>
            <SearchBar/>
            <CardContainer page={page}/>
            <Button label="Show more" handleLoadMore={handleLoadMore}/>
          </div>
        </main>
      <Footer/>
    </>
  )
}