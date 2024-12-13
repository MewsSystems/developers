import { styled } from 'styled-components'
import MovieList from '../movieList/MovieList'
import Card from '../ui/card/Card'
import { useGetNowPlayingMovies } from '../../hooks/movies/useGetNowPlayingMovies'

function NowPlayingMovies() {
  const { isLoading, isError, error, data } = useGetNowPlayingMovies()
  if (isLoading) {
    return (
      <NowPlayingMoviesGrid>
        {Array.from({ length: 6 }).map((_, i) => {
          return <Card key={i} isSkeleton />
        })}
      </NowPlayingMoviesGrid>
    )
  }

  if (isError) return <div>Error: {error.message}</div>

  if (data)
    return (
      <NowPlayingMoviesContainer>
        <h2>Now Playing</h2>
        <NowPlayingMoviesGrid>
          <MovieList movies={data.results} />
        </NowPlayingMoviesGrid>
      </NowPlayingMoviesContainer>
    )
}

const NowPlayingMoviesContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`
const NowPlayingMoviesGrid = styled.div`
  flex-grow: 1;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1rem;
`

export default NowPlayingMovies
