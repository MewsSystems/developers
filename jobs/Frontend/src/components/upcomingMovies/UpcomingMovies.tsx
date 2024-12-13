import { styled } from 'styled-components'
import { useGetUpcomingMovies } from '../../hooks/movies/useGetUpcomingMovies'
import MovieList from '../movieList/MovieList'
import Card from '../ui/card/Card'

function UpcomingMovies() {
  const { isLoading, isError, error, data } = useGetUpcomingMovies()
  if (isLoading) {
    return (
      <UpcomingMoviesGrid>
        {Array.from({ length: 20 }).map((_, i) => {
          return <Card key={i} isSkeleton />
        })}
      </UpcomingMoviesGrid>
    )
  }

  if (isError) return <div>Error: {error.message}</div>

  if (data)
    return (
      <UpcomingMoviesContainer>
        <h2>Upcoming Movies</h2>
        <UpcomingMoviesGrid>
          <MovieList movies={data.results} />
        </UpcomingMoviesGrid>
      </UpcomingMoviesContainer>
    )
}

const UpcomingMoviesContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 1rem;
`
const UpcomingMoviesGrid = styled.div`
  flex-grow: 1;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1rem;
`

export default UpcomingMovies
