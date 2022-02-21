import styled from 'styled-components'
import SearchField from "../components/SearchField";
import SearchResults from "../components/SearchResultsList";

const StyledHomeLayout = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    min-height: 100vh;
    background-color: #fdfdfd;
`

const Home = (props: any) => {

    return (
        <StyledHomeLayout>
            <SearchField />
            <SearchResults />
        </StyledHomeLayout>
    )
}

export default Home
