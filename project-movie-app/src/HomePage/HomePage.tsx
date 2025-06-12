import { Header } from "../Header"
import { SearchBar } from "./HomepageContent/SearchBar"
import { CardContainer } from "./HomepageContent/CardContainer"
import { Button } from "../Button"
import { Footer } from "../Footer"
import { HomePageProps } from "./types"

export const HomePage: React.FC<HomePageProps> = ({searchTerm, setSearchTerm, page, items, handleLoadMore}) => {

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