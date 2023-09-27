import { useNavigate, useParams } from "react-router-dom"
import { Button, Card, Rate, Space, Tag } from "antd"
import { LeftOutlined } from "@ant-design/icons"

import { useGetMovieDetailsQuery } from "../../api/movie"
import {
  AdditionalInfo,
  AdditionalInfoItem,
  Header,
  Label,
  MovieTitle,
  OriginalMovieTitle,
  StyledImage,
} from "./styles"
import { IMAGE_URL_PREFIX } from "../../const"
import { useFailedRequest } from "../../hooks"

export const MovieDetail = () => {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()

  const {
    data: movie,
    isError,
    isFetching,
  } = useGetMovieDetailsQuery(Number(id))

  useFailedRequest(isError)

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
    vote_count,
  } = movie

  return (
    <>
      <Button
        data-testid="backLink"
        icon={<LeftOutlined />}
        type="link"
        onClick={() => navigate(-1)}
      >
        Back
      </Button>
      <Card
      // cover={<MobileCoverImage src={`${IMAGE_URL_PREFIX}${poster_path}`} />}
      >
        <Header>
          <StyledImage src={`${IMAGE_URL_PREFIX}${poster_path}`} />
          <div>
            <MovieTitle>{title}</MovieTitle>
            <OriginalMovieTitle>
              {original_title} ({original_language})
            </OriginalMovieTitle>
            <AdditionalInfo>
              <AdditionalInfoItem>
                <Label>Popularity</Label>
                <Rate disabled defaultValue={popularity / 10} />
              </AdditionalInfoItem>
              <AdditionalInfoItem>
                <Label>Votes</Label>
                {vote_count}
              </AdditionalInfoItem>
              <AdditionalInfoItem>
                <Label>Budget</Label>
                {budget}
              </AdditionalInfoItem>
              <AdditionalInfoItem>
                <Label>Genres</Label>
                <Space size={[0, 8]} wrap>
                  {genres.map((genre) => {
                    return <Tag key={genre.id}>{genre.name}</Tag>
                  })}
                </Space>
              </AdditionalInfoItem>
            </AdditionalInfo>
          </div>
        </Header>
      </Card>
      <Card>{overview}</Card>
    </>
  )
}
