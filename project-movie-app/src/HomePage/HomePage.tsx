import { Header } from "../Header"
import { SearchBar } from "./HomepageContent/SearchBar"
import { CardContainer } from "./HomepageContent/CardContainer"
import { Button } from "../Button"
import { Footer } from "../Footer"
import { HomePageProps } from "./types"
import { ButtonWrapper, Heading} from "./style"
import { Main } from "../styles/Main"

export const HomePage: React.FC<HomePageProps> = ({searchTerm, setSearchTerm, page, items, handleLoadMore}) => {

    return (
        <>
          <Header/>
          <Main>
            <Heading>Popular Movies</Heading>
            <SearchBar searchTerm={searchTerm} setSearchTerm={setSearchTerm}/>
            <CardContainer page={page} items={items}/>
            <ButtonWrapper>
              <Button label="Show more" handleLoadMore={handleLoadMore}/>
            </ButtonWrapper>
          </Main>
          <Footer/>
        </>
    )
}