import { styled } from 'styled-components'
import { useState } from 'react'
import { useDebounce } from '../hooks/utils/useDebounce'

import SearchBar from '../components/ui/searchBar/SearchBar'
import UpcomingMovies from '../components/upcomingMovies/UpcomingMovies'
import NowPlayingMovies from '../components/nowPlayingMovies/NowPlayingMovies'
import SearchedMovies from '../components/searchedMovies/SearchedMovies'

function Home() {
  const [searchQuery, setSearchQuery] = useState('')
  const debouncedSearchQuery = useDebounce(searchQuery, 500)

  return (
    <LayoutContainer id="home">
      <div className="floating-search">
        <SearchBar
          searchQuery={searchQuery}
          setSearchQuery={setSearchQuery}
          placeholder="Search for a movie"
        />
      </div>
      <SearchedMovies query={debouncedSearchQuery} />
      <NowPlayingMovies />
      <UpcomingMovies />
    </LayoutContainer>
  )
}

const LayoutContainer = styled.main`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  margin: 1rem;
  gap: 1rem;

  .floating-search {
    position: absolute;
    top: 122px;
  }

  .home-layout {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
    gap: 1rem;
  }
`

export default Home
