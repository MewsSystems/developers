import React, { useState, useEffect } from "react";
import { useSelector, useDispatch } from "react-redux";
import { reducer } from "../App";
import { Link } from "react-router-dom";
import axios, { AxiosResponse } from "axios";

import styled from "styled-components";
import { FaSearch } from "react-icons/fa";

const SearchBar = styled.div`
  background: white;
  color: #999;
  font-size: 1.5rem;
  border-radius: 2rem;
  width: 98%;
  max-width: 600px;
  display: flex;
  flex-order: column;
  position: relative;
  margin: auto;
  svg {
    position: absolute;
    top: 0.75rem;
    left: 1rem;
  }
  & input {
    background: transparent;
    color: #333;
    border: none;
    border-radius: 2rem;
    font-size: 2rem;
    width: 100%;
    padding: 0.25rem 3rem;
    &:focus {
      background: #red;
    }
  }
  & button {
    font-size: 2rem;
    border: none;
    color: #333;
    background: transparent;
    position: absolute;
    top: 0.25rem;
    right: 0.75rem;
  }
`;

const Results = styled.div`
  padding: 0 3rem 2rem;
  & a {
    color: white;
    text-decoration: none;
    display: flex;
    flex-direction: column;
    min-width: 300px;
    max-width: 550px;
    & div {
      width: 100%;
      padding-bottom: 150%;
      background-color: rgba(0, 0, 0, 0.5);
      background-position: center;
      background-size: cover;
    }
    & p {
      margin-top: 0.5rem;
    }
  }
`;

const Pagination = styled.div`
  margin: auto;
  padding: 2rem 0 4rem 0;
  & button {
    color: white;
    background: transparent;
    border: 1px solid white;
    border-left: none;
    padding: 0.5rem;
    &:first-child {
      border: 1px solid white;
      border-radius: 0.25rem 0 0 0.25rem;
    }
    &:last-child {
      border-radius: 0 0.25rem 0.25rem 0;
    }
    &.active {
      background: white;
      color: #333;
    }
  }
`;

const SearchView = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const dispatch = useDispatch();
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const paginationButtons = [];

  const getButtonClass = (page: number) => {
    if (page === currentPage) {
      return "active";
    }
    return "";
  };

  // If there are more than 10 pages, only show the first 8 pages and the last page
  if (totalPages > 10) {
    for (let i = 1; i <= 8; i++) {
      paginationButtons.push(
        <button
          key={i}
          onClick={() => setCurrentPage(i)}
          className={getButtonClass(i)}
        >
          {i}
        </button>
      );
    }
    paginationButtons.push(
      <button disabled key="ellipsis">
        ...
      </button>
    );
    paginationButtons.push(
      <button
        key={totalPages}
        onClick={() => setCurrentPage(totalPages)}
        className={getButtonClass(totalPages)}
      >
        {totalPages}
      </button>
    );
  }
  // If there are 10 or fewer pages, show all the pages
  else {
    for (let i = 1; i <= totalPages; i++) {
      paginationButtons.push(
        <button
          key={i}
          onClick={() => setCurrentPage(i)}
          className={getButtonClass(i)}
        >
          {i}
        </button>
      );
    }
  }

  const [movies, setMovies] = useState([]);
  const apiKey = "03b8572954325680265531140190fd2a";

  const searchTermFromStore = useSelector((state: any) => state.searchTerm);

  useEffect(() => {
    setSearchTerm(searchTermFromStore);
    if (searchTerm) {
      axios
        .get(
          `https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=${searchTerm}&page=${currentPage}`
        )
        .then((response: AxiosResponse) => {
          setMovies(response.data.results);
          setTotalPages(response.data.total_pages);
        });
    }
  }, [searchTerm, currentPage]);

  const handleSearchInputChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const searchTerm = event.target.value;
    setSearchTerm(searchTerm);
    dispatch({ type: "UPDATE_SEARCH_TERM", searchTerm });
  };

  const handleClear = () => {
    dispatch({ type: "CLEAR_SEARCH_TERM" });
    setSearchTerm("");
    setMovies([]);
    setCurrentPage(1);
    setTotalPages(1);
    //document.getElementsByName("searchBox").focus();
  };

  return (
    <div>
      <header className="text-center">
        <SearchBar>
          <FaSearch />
          <input
            name="searchBox"
            type="text"
            value={searchTerm}
            onChange={handleSearchInputChange}
          />
          {searchTerm !== "" && <button onClick={handleClear}>×</button>}
        </SearchBar>
      </header>
      <Results className="grid">
        {movies &&
          movies.map((movie: any) => (
            <div key={movie.id}>
              <Link to={`/movie/${movie.id}?movieId=${movie.id}`}>
                <div
                  style={{
                    backgroundImage: `url(https://image.tmdb.org/t/p/w200/${
                      (movie as { poster_path: string }).poster_path
                    })`,
                  }}
                />
                <p>{movie.title}</p>
              </Link>
            </div>
          ))}
      </Results>
      {paginationButtons.length > 1 && (
        <Pagination className="text-center">
          {currentPage > 1 && (
            <button onClick={() => setCurrentPage(currentPage - 1)}>
              &lt;
            </button>
          )}
          {paginationButtons}
          {currentPage < totalPages && (
            <button onClick={() => setCurrentPage(currentPage + 1)}>
              &gt;
            </button>
          )}
        </Pagination>
      )}
    </div>
  );
};

export default SearchView;
