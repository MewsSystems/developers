import { useQuery } from '@tanstack/react-query'
import styled from 'styled-components'
import { TfiFaceSad } from 'react-icons/tfi'

import { type Movie } from '@/api/types'
import { Route } from '@/routes/movie/$id'
import { movie } from '@/api/index'
import { TMDB_NOT_FOUND_CODE, imageBaseUrl } from '@/constants/api'

const Wrapper = styled.div`
	display: flex;
	justify-content: center;
	padding: 0 20px;
`
const MovieComponent = styled.div`
	width: 100%;
	max-width: 900px;
	display: grid;
	grid-template-columns: 1fr;
	grid-gap: 10px;

	@media all and (min-width: 500px) {
		grid-template-columns: 1fr 1fr;
	}
`

const Title = styled.h1`
	display: flex;
	align-items: left;
	flex-direction: column;
`

const MetaInfo = styled.div`
	color: grey;
	font-size: 1rem;
	position: relative;
	top: -20px;
`

const NotFoundBlock = styled.div`
	display: flex;
	align-items: center;
	flex-direction: column;
	width: 100%;
`
const ImgStyled = styled.img`
	width: 100%;
`

const Tag = styled.div`
	padding: 5px 10px;
	background: #04AA6D;
	color: #fff;
	border-radius: 10px;
	margin-right: 10px;
	margin-top: 5px;
	font-size: 1rem;
	display: inline-block;
`

export default function Movie() {
	const { id } = Route.useParams()
	
	const { data, isLoading } = useQuery({
		queryKey: ['movie', { id }],
		staleTime: Infinity,
		queryFn: () => movie({ id: Number(id) })
	})

	if (isLoading) return <h1>LOADING ...</h1>

	if (data && data.status_code === TMDB_NOT_FOUND_CODE) return (
		<NotFoundBlock>
			<TfiFaceSad size={'2em'} />
			<h1>Movie is not found</h1>
		</NotFoundBlock>
	)

	const releaseYear = data && new Date(data.release_date).getFullYear()

	return data && (
		<Wrapper>
			<MovieComponent>
				<ImgStyled src={`${imageBaseUrl}original/${data.poster_path}`} alt={`poster for ${data.title}`} />
				<div>
					<Title>
						{ data.title } ({ releaseYear })
					</Title>
					<MetaInfo>
						<div>id: { data.id }</div>
					</MetaInfo>
					<p>
						{data.overview}
					</p>
					<div>
						{data.genres.map(genre => {
							return (
								<Tag>{ genre.name }</Tag>
							)
						})}
					</div>
				</div>
			</MovieComponent>
		</Wrapper>
	)
}
