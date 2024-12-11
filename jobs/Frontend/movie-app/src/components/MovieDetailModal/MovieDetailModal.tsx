import React, { memo, useEffect } from "react";
import {
  StyledMovieDetailModalContainer,
  StyledMovieDetailModalHeader,
  StyledMovieDetailModalHeadline,
  StyledMovieDetailModalImage,
  StyledMovieDetailModalParagraph,
  StyledMovieDetailModalWrapper,
} from "./MovieDetailModal.styles";
import { StyledCloseButton } from "../CloseButton/CloseButton.styles";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../../state/store";
import { fetchMovieDetail } from "../../state/movies/moviesSlice";
import { formatDate } from "../../helpers";

const MovieDetailModal = ({
  movieId,
  onCloseModal,
}: {
  movieId: string;
  onCloseModal: () => void;
}) => {
  const dispatch = useDispatch<AppDispatch>();
  const { movieDetail } = useSelector((state: RootState) => state.movies);

  useEffect(() => {
    dispatch(fetchMovieDetail({ movieId }));
  }, [movieId, dispatch]);

  const movieDetailData = movieDetail[movieId];

  if (!movieDetailData) {
    return null;
  }

  const imageUrl = movieDetailData.poster_path
    ? `https://image.tmdb.org/t/p/w500${movieDetailData.poster_path}`
    : null;
  const releaseDate = formatDate(movieDetailData.release_date);
  const genres = movieDetailData.genres.map((genre) => genre.name).join(", ");

  return (
    <>
      <StyledMovieDetailModalHeader>
        <StyledCloseButton onClick={onCloseModal} />
      </StyledMovieDetailModalHeader>
      <StyledMovieDetailModalContainer>
        {imageUrl && (
          <StyledMovieDetailModalImage
            src={imageUrl}
            alt={movieDetailData.title}
          />
        )}
        <StyledMovieDetailModalWrapper>
          <StyledMovieDetailModalHeadline>
            {movieDetailData.title}
          </StyledMovieDetailModalHeadline>
          {releaseDate && releaseDate !== "Invalid Date" && (
            <StyledMovieDetailModalParagraph>
              Release date: {releaseDate}
            </StyledMovieDetailModalParagraph>
          )}
          {movieDetailData.vote_count !== 0 && (
            <StyledMovieDetailModalParagraph>
              Rating: {movieDetailData.vote_average}
            </StyledMovieDetailModalParagraph>
          )}
          {genres && (
            <StyledMovieDetailModalParagraph>
              Genres: {genres}
            </StyledMovieDetailModalParagraph>
          )}
          {movieDetailData.runtime !== null ||
            (movieDetailData.runtime !== 0 && (
              <StyledMovieDetailModalParagraph>
                Runtime: {movieDetailData.runtime} minutes
              </StyledMovieDetailModalParagraph>
            ))}
          {movieDetailData.overview && (
            <StyledMovieDetailModalParagraph>
              {movieDetailData.overview}
            </StyledMovieDetailModalParagraph>
          )}
        </StyledMovieDetailModalWrapper>
      </StyledMovieDetailModalContainer>
    </>
  );
};

export default memo(MovieDetailModal);
