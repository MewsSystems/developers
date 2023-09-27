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
    originalLanguage,
    originalTitle,
    overview,
    popularity,
    posterPath,
    title,
    voteCount,
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
      <Card>
        <Header>
          <StyledImage src={`${IMAGE_URL_PREFIX}${posterPath}`} />
          <div>
            <MovieTitle>{title}</MovieTitle>
            <OriginalMovieTitle>
              {originalTitle} ({originalLanguage})
            </OriginalMovieTitle>
            <AdditionalInfo>
              <AdditionalInfoItem>
                <Label>Popularity</Label>
                <Rate disabled defaultValue={popularity / 10} />
              </AdditionalInfoItem>
              <AdditionalInfoItem>
                <Label>Votes</Label>
                {voteCount}
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
