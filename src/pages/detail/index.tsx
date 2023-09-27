import { useNavigate, useParams } from "react-router-dom"
import { useGetMovieDetailsQuery } from "../../api/movie"
import { Button } from "antd"
import { LeftOutlined } from "@ant-design/icons"
import { Header, StyledImage } from "./styles"
import { IMAGE_URL_PREFIX } from "../../const"

export const MovieDetail = () => {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()

  const { data: movie, isFetching } = useGetMovieDetailsQuery(Number(id))

  if (isFetching) {
    return <div>Loading...</div>
  }

  if (!movie) {
    return <div>Movie not found</div>
  }

  const {
    budget,
    genres,
    original_language,
    original_title,
    overview,
    popularity,
    poster_path,
    title,
  } = movie

  return (
    <div>
      <Button icon={<LeftOutlined />} type="link" onClick={() => navigate(-1)}>
        Back
      </Button>
      <Header>
        <StyledImage src={`${IMAGE_URL_PREFIX}${poster_path}`} />
        <div>{title}</div>
        <div>
          {original_title} ({original_language})
        </div>
        <div>
          <div>Popularity</div>
          {popularity}
        </div>
        <div>
          <div>Budget</div>
          {budget}
        </div>
        <div>
          {genres.map((genre) => {
            return <div>{genre.name}</div>
          })}
        </div>
      </Header>
      <div>{overview}</div>
    </div>
  )
}
