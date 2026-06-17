import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Pagination from '../Pagination';
import MovieCard from './MovieCard';

function Search() {
  const navigate = useNavigate();
  const [movies, setMovies] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const [postPerPage] = useState(12);
  const [totalPages, setTotalPages] = useState(1); 

  useEffect(() => {
    const apiKey = '03b8572954325680265531140190fd2a';

    // Fetch 12 random movies on load or when searchTerm changes
    if (currentPage === 1 && searchTerm === '') {
      fetch(`https://api.themoviedb.org/3/discover/movie?api_key=${apiKey}`)
        .then(response => response.json())
        .then(data => {
          setMovies(data.results);
          setTotalPages(data.total_pages); // Update total pages
        })
        .catch(error => console.error('Error fetching random movies:', error));
    } else if (searchTerm !== '') {
      // Fetch movies based on the search term and current page
      fetch(`https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=${searchTerm}`)
        .then(response => response.json())
        .then(data => {
          setMovies(data.results);
          setTotalPages(data.total_pages); 
        })
        .catch(error => console.error('Error fetching movies:', error));
    }
  }, [currentPage, searchTerm]);

  const handleMovieClick = (movieId) => {
    navigate(`/movie/${movieId}`);
  };

  const paginate = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const indexOfLastPost = currentPage * postPerPage;
  const indexOfFirstPost = indexOfLastPost - postPerPage;
  const currentMovies = movies.slice(indexOfFirstPost, indexOfLastPost);

  return (
    <div>
      <div className="search-input">
        <input
          type="text"
          placeholder="Search movies..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className='styled-input'
        />
      </div>
      <div className="row movie-section">
        {currentMovies.map((movie) => (
          <div key={movie.id} className="col-md-2">
            <MovieCard
              movie={movie}
              onClick={() => handleMovieClick(movie.id)}
            />
          </div>
        ))}
      </div>
      <Pagination
        currentPage={currentPage}
        totalPages={totalPages}
        postPerPage={postPerPage}
        paginate={paginate}
      />
    </div>
  );
}

export default Search;
