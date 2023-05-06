import { Col, Row } from "antd";
import React from "react";
import type { MovieDetailsType } from "../../store/movieDetails/types";
import { ImageStyled } from "./MovieDetailsPage.styled";
import { MovieInfo } from "./MovieInfo";
import { MovieInfoProps } from "./MovieInfo/MovieInfo";

type MovieDetailsPageViewProps = MovieDetailsType & { isLoading?: boolean };

const MovieDetailsPageView = (props: MovieDetailsPageViewProps) => {
  const { isLoading, imgUrl, title } = props;

  if (isLoading) {
    return <div>Loading...</div>;
  }

  return (
    <Row>
      <Col span={8}>
        <ImageStyled src={imgUrl} alt={title} />
      </Col>
      <MovieInfo {...(props as MovieInfoProps)} />
    </Row>
  );
};

export default React.memo(MovieDetailsPageView);
