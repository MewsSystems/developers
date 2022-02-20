import HomeLayout from "../components/HomeLayout";
import SearchField from "../components/SearchField";
import SearchResults from "../components/SearchResultsList";

const Home = (props: any) => {

    const { currPage, setCurrPage } = props

    return (
        <HomeLayout>
            <SearchField />
            <SearchResults />
        </HomeLayout>
    )
}

export default Home
