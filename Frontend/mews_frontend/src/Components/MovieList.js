import React, { useEffect, useState } from 'react';
import styled from 'styled-components';

import Movie from './Movie';
import Pagination from './Pagination'
import { connect } from 'react-redux';
import { getMovie, searchMovie, fetchMovies } from '../actions/movieAction';

const MovieListStyle = styled.div`
display:flex;
justify-content:center;
flex-wrap:wrap;
`;
const Input = styled.input`
width:50%;
border:none;
height:2.5em;
border-radius:5em;
margin: 1.5em 0;
padding: 0 1em;
&:focus {
    outline: none;
    box-shadow: 0px 0px 2px red;
}

`
const Container = styled.div`
 display:flex;
 flex-direction:column;
 justify-content:center;
 align-item:center;
`

const MovieList = ({ movies, text, getMovie, searchMovie, fetchMovies }) => {

    const [currentPage, setCurrentPage] = useState(1);
    const [postsPerPage] = useState(8);

    useEffect(() => {
        getMovie()
        // eslint-disable-next-line react-hooks/exhaustive-deps

    }, [])

    // Get current movies
    const indexOfLastMovie = currentPage * postsPerPage;
    const indexOfFirstMovie = indexOfLastMovie - postsPerPage;
    const currentMovies = movies.slice(indexOfFirstMovie, indexOfLastMovie);

    // Change page
    const paginate = pageNumber => setCurrentPage(pageNumber);

    // Search 
    const handleOnchange = e => {
        e.preventDefault()
        searchMovie(e.target.value)
    }
    const onSubmit = e => {
        e.preventDefault()
        fetchMovies(text)
    }
    return (
        <>
            <Container>
                <form onSubmit={onSubmit}>
                    <Input
                        type='text'
                        name='search'
                        value={text}
                        placeholder='find your movie here...'
                        onChange={handleOnchange}

                    />
                </form>

                <MovieListStyle>

                    {currentMovies.map(movie => (
                        <Movie movie={movie} key={movie.id} />
                    ))}
                </MovieListStyle>

                <Pagination
                    postsPerPage={postsPerPage}
                    totalPosts={movies.length}
                    paginate={paginate} />

            </Container>
        </>
    )
}

const mapStateToProps = state => ({
    movies: state.movie.movies,
    loading: state.movie.loading,
    text: state.movie.text
})
export default connect(mapStateToProps, { getMovie, searchMovie, fetchMovies })(MovieList);
