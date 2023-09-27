import { useNavigate, useParams } from "react-router-dom"
import { Card, Rate, Space, Spin, Tag } from "antd"
import { LeftOutlined } from "@ant-design/icons"

import { useGetMovieDetailsQuery } from "../../api/movie"
import { IMAGE_URL_PREFIX } from "../../const"
import { useFailedRequest } from "../../hooks"
import {
  AdditionalInfo,
  AdditionalInfoItem,
  BackButton,
  Header,
  HeaderContent,
  Label,
  MovieTitle,
  OriginalMovieTitle,
  SpinWrapper,
  StyledImage,
} from "./styles"

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
    return (
      <SpinWrapper>
        <Spin size="large" />
      </SpinWrapper>
    )
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
      <BackButton
        data-testid="backLink"
        icon={<LeftOutlined />}
        type="link"
        onClick={() => navigate(-1)}
      >
        Back
      </BackButton>
      <Header>
        <HeaderContent>
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
        </HeaderContent>
      </Header>
      <Card>{overview}</Card>
    </>
  )
}
