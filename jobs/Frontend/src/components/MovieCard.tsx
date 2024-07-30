import styled from 'styled-components'
import { Link } from '@tanstack/react-router'

import { routeNames } from '@/constants/routeNames'
import { imageBaseUrl } from '@/constants/api'
import { type Movie } from '@/api/types'

type Props = {
	movie: Movie
}

const imageWidth = 500

const Wrapper = styled.div`
	border: 1px solid grey;
	width: 100%;
	border-radius: 10px;
	overflow: hidden;
	height: 100%;
	position: relative;

	&:focus-within,
	&:hover {
		h3 { background: #04AA6D; }
		a {color: #fff !important;}
	}
`
const ImgStyled = styled.img`
	width: 100%;
`

const TitleBlock = styled.h3`
	width: 100%;
	position: absolute;
	bottom: 0;
	background: #fff;
	min-height: 50px;
	margin: 0;
	display: flex;
	align-items: center;
	justify-content: center;
`
const MovieLink = styled(Link)`
	text-decoration: none;
	color: #000;
`

export default function MovieCard({ movie }: Props) {
	const releaseYear = movie && new Date(movie.release_date).getFullYear()
	return (
		<Wrapper>
			<Link tabIndex={-1} to={routeNames.movie} params={{ id: movie.id.toString() }}>
				<ImgStyled src={`${imageBaseUrl}w${imageWidth}${movie.poster_path}`} alt={`poster for ${movie.title}`} />
			</Link>
			<TitleBlock>
				<MovieLink to={routeNames.movie} params={{ id: movie.id.toString() }}>
					{movie.title} ({releaseYear})
				</MovieLink>
			</TitleBlock>
		</Wrapper>
	)
}
