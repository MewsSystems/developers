import React from "react";
import {
  ListItem,
  Paragraph,
  Header,
  ThumbnailImage,
  VerticalDiv,
  MovieDetailDiv
} from "../styles/Style.js";
import Moment from "react-moment";
import { Link } from "react-router-dom";
import Loader from "react-loader-spinner";

const MovieListItem = ({
  movie: { title, release_date, poster_path, overview },
  clickHandler
}) => {
  return (
    <React.Fragment>
      <Link to={`/moviedetail`}>
        <ListItem onClick={clickHandler}>
          <VerticalDiv>
            <ThumbnailImage
              src={[`https://image.tmdb.org/t/p/w500${poster_path}`]}
              loader={
                <Loader
                  type="Rings"
                  color="lightblue"
                  height={200}
                  width={133}
                />
              }
            />
            <MovieDetailDiv>
              <Header>{title}</Header>
              <Paragraph>
                <Moment format="YYYY">{release_date}</Moment>
              </Paragraph>
              <Paragraph>
                {overview.length < 300
                  ? overview
                  : overview.substring(0, 300) + "..."}
              </Paragraph>
            </MovieDetailDiv>
          </VerticalDiv>
        </ListItem>
      </Link>
    </React.Fragment>
  );
};

export default MovieListItem;
