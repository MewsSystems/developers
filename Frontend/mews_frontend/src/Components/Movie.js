import React from 'react';
import styled from 'styled-components';
import { Link } from "react-router-dom";

const Img = styled.img`
 height:200px;
 width:300px;
 object-fit:contain;   
`;
const MovieStyle = styled.div`
 color:#fff;
 width:300px;
 height:250px;
 padding:0.5em;
 transition:transform 100ms;
 &:hover {
     transform:scale(1.09)
 };
`;

const base_url = 'https://image.tmdb.org/t/p/original/';

const Movie = ({ movie }) => {
    return (

        <Link to={'/movie/' + movie.id} key={movie.id}>
            <MovieStyle>
                <Img src={`${base_url}${movie.backdrop_path || movie.poster_path}`}
                    alt='' />
                <p>{movie.title}</p>
            </MovieStyle>
        </Link>

    )
}

export default Movie
