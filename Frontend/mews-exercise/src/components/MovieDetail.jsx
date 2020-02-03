import React, { useState, useEffect } from "react";
import {
  Paragraph,
  Header,
  Button,
  PosterImage,
  VerticalDiv,
  MovieDetailDiv,
  BackButton
} from "../styles/Style.js";
import APIHelper from "../Helpers/APIHelper.js";
import Moment from "react-moment";
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";

const MovieDetail = () => {
  const [movieDetail, setMovieDetail] = useState({});

  const selectedMovieId = useSelector(state => state.selectedMovieId);

  const fetchMovieDetail = async id => {
    const response = await APIHelper.fetchMovieDetail(id);
    setMovieDetail(response);
  };

  const moneyFormatter = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 2
  });

  useEffect(() => {
    fetchMovieDetail(selectedMovieId);
  }, [selectedMovieId]);

  if (selectedMovieId === 0) {
    return (
      <React.Fragment>
        <Link to={`/`}>
          <BackButton>Back</BackButton>
        </Link>
        <VerticalDiv>
          <Header>Please search a movie first...</Header>)
        </VerticalDiv>
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <Link to={`/`}>
        <Button style={{ position: "absolute", left: "1%" }}>Back</Button>
      </Link>
      <VerticalDiv>
        <PosterImage
          src={[
            `https://image.tmdb.org/t/p/w500${movieDetail.data &&
              movieDetail.data.poster_path}`
          ]}
        />
        <MovieDetailDiv>
          <Header>{movieDetail.data && movieDetail.data.title}</Header>
          <Paragraph>
            <Moment format="YYYY">
              {movieDetail.data && movieDetail.data.release_date}
            </Moment>
          </Paragraph>
          <Paragraph>{movieDetail.data && movieDetail.data.overview}</Paragraph>
          <Paragraph>{movieDetail.data && movieDetail.data.homepage}</Paragraph>
          <Paragraph>
            Average rating: {movieDetail.data && movieDetail.data.vote_average}{" "}
            / 10
          </Paragraph>
          <Paragraph>
            Budget:{" "}
            {movieDetail.data && moneyFormatter.format(movieDetail.data.budget)}
          </Paragraph>
          <Paragraph>
            Genre:
            {movieDetail.data &&
              movieDetail.data.genres.map(genre => genre.name + " ")}
          </Paragraph>
          <Paragraph>
            Produced in:
            {movieDetail.data &&
              movieDetail.data.production_countries.map(
                company => company.name + " "
              )}
          </Paragraph>
        </MovieDetailDiv>
      </VerticalDiv>
    </React.Fragment>
  );
};

export default MovieDetail;
