import { Header } from './Header/Header'
import "./Header/header.style.css"
import { SearchBar } from './Content/SearchBar'
import "./Content/SearchBar.style.css"
import { CardContainer } from './Content/CardContainer'

export const App = () => {

  return (
    <>
      <Header/>
        <main>
          <div className="app__container">
            <h2>Popular Movies</h2>
            <SearchBar/>
            <CardContainer/>
          </div>
        </main>
    </>
  )
}