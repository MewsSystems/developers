import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useLocation } from "react-router-dom";
import { Link } from "react-router-dom";
import { useHistory } from "react-router-dom";
import styled from "styled-components";
import { FaChevronLeft } from "react-icons/fa";

const MovieDetails = styled.div`
  display: flex;
  flex-direction: column;
  position: relative;
  z-index: 1;
`;
const Loading = styled.div`
  display: flex;
  width: 100vw;
  height: 100vh;
  align-items: center;
  justify-content: center;
  opacity: 0.7;
  font-size: 2 rem;
`;

const Back = styled.button`
  font-size: 1.65rem;
  border-radius: 2rem;
  padding: 0.25rem 0.25rem;
  width: 2.5rem;
  height: 2.5rem;
  line-height: 2rem;
  margin: 0 1rem;
  color: white;
  background: transparent;
  border: 2px solid white;
  display: inline-flex;
`;
const Details = styled.div`
  padding: 0 1.5rem 2rem;
  display: flex;

  flex-direction: column;
  align-items: flex-start;
  gap: 2rem;
  max-width: 1200px;
  margin: auto;

  & .poster-props {
    display: flex;
    justify-content: center;
    flex-direction: column;
    & img.poster {
      width: 100%;
      max-width: 450px;
    }
  }
  @media (min-width: 500px) and (max-width: 719px) {
    .poster-props {
      display: flex;
      flex-direction: row;
      & img.poster {
        width: 50%;
      }
    }
  }
  @media (min-width: 720px) {
    flex-wrap: wrap;
    flex-direction: row;
    & .title-summary {
      flex: 0 0 65%;
      order: 2;
      align-self: flex-start;
    }
    & .poster-props {
      flex: 0 0 30%;
      order: 1;
      & img.poster {
        width: 100%;
      }
    }
  }
  @media (min-width: 1000px) {
    & .title-summary {
      flex: 0 0 70%;
    }
    & .poster-props {
      flex: 0 0 25%;
    }
  }
`;
const Polaroid = styled.div`
  background: rgba(255, 255, 255, 0.8);
  padding: 1rem;
  border-radius: 0.5rem;
  text-align: left;
  box-shadow: 0 2px 3px rgba(0, 0, 0, 0.7);
  order: 1;
  & img {
    width: 100%;
  }
  & p {
    margin-bottom: 0;
    color: #333;
  }
`;

const MovieProperties = styled.div`
  text-shadow: 0 1px 3px black;
  font-weight: bold;
  & p {
    display: flex;
    flex-direction: row;
    & .prop {
      font-weight: normal;
      display: inline-block;
      margin-right: 0.75rem;
      text-align: right;
      width: 5rem;
      min-width: 5rem;
      opacity: 0.8;
    }
    & .value {
      display: inline-block;
    }
  }
`;

const MovieDetailView = () => {
  const location = useLocation();
  const movieId = new URLSearchParams(location.search).get("movieId");
  const history = useHistory();
  const [isLoading, setIsLoading] = useState(true);
  const [movie, setMovie] = useState<{}>({});

  useEffect(() => {
    const fetchMovie = async () => {
      const apiKey = "03b8572954325680265531140190fd2a";
      const response = await fetch(
        `https://api.themoviedb.org/3/movie/${movieId}?api_key=${apiKey}`
      );
      const data = await response.json();
      setMovie(data);
      setIsLoading(false);
    };
    fetchMovie();
  }, [movieId]);

  if (isLoading) {
    return <Loading>Loading...</Loading>;
  }

  const releaseDate = (movie as { release_date: string }).release_date;
  const year = releaseDate ? releaseDate.slice(0, 4) : "Unknown";
  const score = (movie as { vote_average: number }).vote_average;

  const countries = (
    movie as {
      production_countries: { iso_3166_1: string; name: string }[];
    }
  ).production_countries;
  const countryNames = countries
    ? countries.map((country) => country.name).join(", ")
    : "Unknown";
  const runtime = (movie as { runtime: number }).runtime;
  const genres = (movie as { genres: { name: string }[] }).genres;
  const genreElements = genres.map((genre: { name: string }, index: number) => {
    if (index === genres.length - 1) {
      return <b key={genre.name}>{genre.name}</b>;
    }
    return <b key={genre.name}>{genre.name}, </b>;
  });

  const backdropPath = (movie as { backdrop_path: string }).backdrop_path;
  const backdropUrl = `https://image.tmdb.org/t/p/w500/${backdropPath}`;

  const BackDropBG = styled.div`
    display: flex;
    background-image: url(${backdropUrl});
    background-size: cover;
    background-position: center;
    filter: blur(15px) brightness(40%);
    box-shadow: inset 0 20vh 33vw black;
    width: 100vw;
    height: 100vh;
    z-index: 0;
    position: fixed;
    top: 0;
    left: 0;
  `;

  return (
    <div>
      <BackDropBG />
      <MovieDetails>
        <header className="flex-row">
          <Back onClick={() => history.goBack()}>
            <FaChevronLeft />
          </Back>
        </header>
        <Details>
          <div className="title-summary">
            <h1>
              {(movie as { title: string }).title}
              <small> ({year})</small>
            </h1>
            <Polaroid>
              <img src={backdropUrl} alt="Movie backdrop" />
              <p>{(movie as { overview: string }).overview}</p>
            </Polaroid>
          </div>
          <div className="poster-props">
            {(movie as { poster_path: string }).poster_path && (
              <img
                className="poster"
                src={`https://image.tmdb.org/t/p/w500/${
                  (movie as { poster_path: string }).poster_path
                }`}
                alt={(movie as { title: string }).title}
              />
            )}
            <MovieProperties>
              <p>
                <span className="prop">Year</span>
                <span className="value">{year}</span>
              </p>
              <p>
                <span className="prop">Score</span>
                <span className="value">{score} / 10</span>
              </p>
              <p>
                <span className="prop">Runtime</span>
                <span className="value">{runtime} minutes</span>
              </p>
              <p>
                <span className="prop">Country</span>
                <span className="value">{countryNames}</span>
              </p>
              <p>
                <span className="prop">Genres</span>
                <span className="value">{genreElements}</span>
              </p>
            </MovieProperties>
          </div>
        </Details>
      </MovieDetails>
    </div>
  );
};

export default MovieDetailView;
