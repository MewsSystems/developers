import styled from 'styled-components'
import { useRouterState } from '@tanstack/react-router'

import { type Movie } from '@/api/types'
import MovieCard from './MovieCard'

type Props = {
	movies: Movie[]
}

const Wrapper = styled.div`
	padding: 0 20px;
	display: flex;
	flex-direction: column;
	align-items: center;
`
const List = styled.ul`
	padding: 0;
	margin: 0;
	list-style: none;
	display: grid;
	grid-template-columns: 1fr;
	column-gap: 10px;
	row-gap: 10px;
	max-width: 900px;
	width: 100%;

	@media all and (min-width: 769px) {
		grid-template-columns: 1fr 1fr;
	}

	@media all and (min-width: 900px) {
		grid-template-columns: 1fr 1fr 1fr;
	}
`

const ListItem = styled.li`
	
`

export default function MoviesList(props: Props) {
	const router = useRouterState()

	return (
		<Wrapper>
			{!router.location.search.query && <h1>Trending movies:</h1>}
			{router.location.search.query && <h1>Search result:</h1>}
			<List>
				{props.movies.map(movie => {
					return (
						<ListItem key={movie.id}>
							<MovieCard movie={movie} />
						</ListItem>
					)
				})}
			</List>
		</Wrapper>
	)
}
