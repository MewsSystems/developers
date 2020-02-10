import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { useParams, useHistory } from "react-router-dom";

import { AppState } from "../../reducers";
import { Spinner } from "../../components/spinner";
import { movieLoad } from "../../actions/movie-detail";
import { formatMoney, formatDuration } from "../../utils";
import { Card, Content, Title, SubTitle, Paragraph, ErrorMessage, Button } from "../../components";
import { Poster, Fact, CompanyInfo } from "./components";

export const MovieView: React.FC = () => {
  const { id } = useParams()
  const dispatch = useDispatch();
  const { error, isFetching, movie } = useSelector(({ movieDetail }: AppState) => ({ ...movieDetail }))
  const history = useHistory()

  if (!id) {
    return (<ErrorMessage>Something went wrong, please go to the main page search for movies</ErrorMessage>)
  }

  if (error) {
    return <ErrorMessage>{error}</ErrorMessage>
  }

  if (isFetching) {
    return <Spinner />
  }

  if (!movie || movie.id != Number(id)) {
    dispatch(movieLoad(Number(id)));
    return <Spinner />
  }

  const goBackHandler = () => {
    history.push('/');
  }

  return (
    <>
      <Button onClick={goBackHandler}>Go Back</Button>
      <Card>
        <Poster fileName={movie.poster_path} />
        <Content >
          <Title>{movie.title}</Title>
          <SubTitle>{movie.tagline}</SubTitle>
          <Paragraph>{movie.overview}</Paragraph>
          <Fact label="Website" value={movie.homepage} />
          <Fact label="Original Language" value={movie.original_language} />
          <Fact label="Popularity" value={movie.popularity} />
          <Fact label="Production Companies" value={movie.production_companies.map(q => <CompanyInfo {...q} />)} />
          <Fact label="Country" value={movie.production_countries.map(q => q.name).join(', ')} />
          <Fact label="Released" value={movie.release_date} />
          <Fact label="Budget" value={formatMoney(movie.budget)} />
          <Fact label="Revenue" value={formatMoney(movie.revenue)} />
          <Fact label="Duration" value={formatDuration(movie.runtime)} />
          <Fact label="Status" value={movie.status} />
          <Fact label="Genres" value={movie.genres.map(q => q.name).join(', ')} />
        </Content>
      </Card>
    </>
  )
}
