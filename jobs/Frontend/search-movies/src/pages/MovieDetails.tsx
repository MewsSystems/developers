import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import styled from "styled-components";
import { shadowSm } from "../components/General";
import GenreChips from "../components/GenreChips";
import MovieRatings from "../components/MovieRatings";
import { API_KEY, BASE_IMG_URL } from "../config/api";
import { axiosBrowse } from "../config/axios";
import { useAppDispatch, useAppSelector } from "../store/hooks";
import { fetchMovies } from "../store/reducers/BrowseMoviesReducer";
import { colors, device } from "../utils/theme";
import { Movie } from "../utils/types";

const MovieDetailsContainer = styled.div`
  margin-top: 40px;
  display: flex;
  justify-content: center;
  flex-direction: column;
  @media ${device.tablet} {
    display: flex;
    flex-direction: row;
    justify-content: center;
  }
`;

const MovieBannerImg = styled.img`
  height: auto;
  width: 18rem;
  margin: 10px;
`;

const MovieBannerContainer = styled.div`
  margin: 0 auto;
`;

const MovieInfo = styled.div`
  box-shadow: ${shadowSm};
  margin: 10px;
  min-height: 100%;
  padding: 20px;
`;

const MovieTitle = styled.h1`
  color: ${colors.primaryText};
`;

const MovieOverview = styled.p`
  color: ${colors.primaryText};
`;

const MovieDetailsLoading = styled.div`
  color: ${colors.primaryText};
  margin: 40px 0;
  display: flex;
  justify-content: center;
`;

interface MovieDetailsParams {
  currentPage: string;
  movieId: string;
  [x: string]: string | undefined;
}

const MovieDetails = () => {
  const { currentPage, movieId } = useParams<MovieDetailsParams>();
  const [movie, setMovie] = useState<Movie>();
  const browseMovies = useAppSelector((state) => state.browseMovies);
  const dispatch = useAppDispatch();

  // Fetch movie details if not in available redux state
  useEffect(() => {
    (async function () {
      if (currentPage) {
        if (!browseMovies.pages[currentPage]) {
          await dispatch(fetchMovies(currentPage));
        }
        const movieList = browseMovies.pages[currentPage];
        const movie = movieList.find(
          (movie: any) => movie.id === Number(movieId)
        );
        setMovie(movie);
      }
      if (!currentPage) {
        try {
          const movieDetails = await axiosBrowse.get(`/movie/${movieId}`, {
            params: {
              api_key: API_KEY,
            },
          });
          setMovie(movieDetails.data as unknown as Movie);
        } catch (error) {}
      }
    })();
  }, [browseMovies, movieId]);

  return (
    <>
      {movie ? (
        <MovieDetailsContainer data-testid="movie-details-container">
          <MovieBannerContainer>
            <MovieBannerImg src={`${BASE_IMG_URL}${movie.poster_path}`} />
          </MovieBannerContainer>
          <MovieInfo>
            <MovieTitle>{movie.original_title}</MovieTitle>
            <GenreChips genreIds={movie.genre_ids ?? movie.genres}></GenreChips>
            <MovieOverview>{movie.overview}</MovieOverview>
            <MovieRatings
              rating={movie.vote_average}
              ratingCount={movie.vote_count}
            />
          </MovieInfo>
        </MovieDetailsContainer>
      ) : (
        <MovieDetailsLoading>Loading...</MovieDetailsLoading>
      )}
    </>
  );
};

export default MovieDetails;
