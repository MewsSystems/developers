import React, { useState, useEffect } from "react";
import styled from "styled-components";
import arrow from "./arrow.svg";
import axios from "axios";

const API_KEY = "03b8572954325680265531140190fd2a";
const API_URL = "https://api.themoviedb.org/3/search/movie";

const StyledFormLabel = styled.label`
  color: black;
  display: flex;
  align-items: center;
  justify-content: start;
  font-size: calc(10px + 2vmin);
  margin: calc(10px + 2vmin);
`;

const Input = styled.input`
  position: sticky;
  width: 60%;
  height: calc(10px + 2vmin);
  font-size: calc(10px + 1vmin);
  &:not(:focus) {
    box-shadow: 0 0 2px 2px #ff6a00;
  }
`;

const MovieList = styled.ul`
  list-style: none;
  padding: 0;
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
`;

const MovieItem = styled.li`
  margin: 20px;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  width: 25%;
  font-size: 2vmin;
`;

const Poster = styled.img`
  height: 30vmin;
`;

const LoadMoreButton = styled.button`
  padding: 2vmin 3vmin;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 25px;
  cursor: pointer;
  margin-bottom: 5vmin;
  font-size: calc(10px + 2vmin);

  &:hover {
    background-color: #ff6a00;
  }
`;

const ScrollToTopButton = styled.button`
  position: fixed;
  bottom: 20px;
  right: 20px;
  padding: 2vmin;
  background-color: #ff6a00;
  color: white;
  border: none;
  border-radius: 50%;
  cursor: pointer;
  transition: background-color 0.3s ease;
  width: 8vmin;
  height: 8vmin;
  display: ${(props) => (props.show ? "block" : "none")};

  &:hover {
    background-color: #0056b3;
  }

  & > img {
    width: 4vmin;
    height: 4vmin;
  }
`;

export const Form = () => {
  const [query, setQuery] = useState("");
  const [page, setPage] = useState(1);
  const [movies, setMovies] = useState([]);
  const [hasMore, setHasMore] = useState(false);
  const [showScrollButton, setShowScrollButton] = useState(false);

  useEffect(() => {
    if (query.length > 0) {
      const fetchMovies = async () => {
        try {
          const response = await axios.get(API_URL, {
            params: {
              api_key: API_KEY,
              query: query,
              page: page,
            },
          });

          if (page === 1) {
            setMovies(response.data.results);
          } else {
            setMovies((prevMovies) => [
              ...prevMovies,
              ...response.data.results,
            ]);
          }
          console.log(response.data.total_pages);
          setHasMore(page < response.data.total_pages);
        } catch (error) {
          console.error("Error fetching data:", error);
        }
      };

      fetchMovies();
    } else {
      setMovies([]);
    }
  }, [query, page]);

  useEffect(() => {
    const handleScroll = () => {
      if (window.pageYOffset > 0) {
        setShowScrollButton(true);
      } else {
        setShowScrollButton(false);
      }
    };

    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  const handleInputChange = (e) => {
    setQuery(e.target.value);
  };

  const handleLoadMore = () => {
    setPage((prevPage) => prevPage + 1);
  };

  const scrollToTop = () => {
    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  };

  return (
    <>
      <form onSubmit={(e) => e.preventDefault()}>
        <StyledFormLabel htmlFor="movie-input">
          What would you like to find?
        </StyledFormLabel>
        <Input
          id="movie-input"
          type="text"
          placeholder="Type your search here"
          value={query}
          onChange={handleInputChange}
        />
      </form>
      <MovieList>
        {movies.map((movie) => (
          <MovieItem key={movie.id}>
            <Poster
              src={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
            />
            <h2>{movie.title}</h2>
            <p>{movie.overview}</p>
          </MovieItem>
        ))}
      </MovieList>
      {hasMore && (
        <LoadMoreButton onClick={handleLoadMore}>Load More</LoadMoreButton>
      )}
      <ScrollToTopButton
        onClick={scrollToTop}
        show={showScrollButton}
        title="Scroll to Top"
      >
        <img src={arrow} />
      </ScrollToTopButton>
    </>
  );
};
