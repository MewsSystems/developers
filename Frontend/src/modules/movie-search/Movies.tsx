import React from 'react';
import { Link } from 'react-router-dom';
import styled from 'styled-components';
import { useAppSelector } from '../../store';

const Ul = styled.ul`
    padding: 0;
    li {
        list-style: none;
    }
    a {
        color: #444;
        display: inline-block;
        padding: 2px 0;
        text-decoration: none;
        
        &:hover {
            color: black;
        }
    }
`;

export default function Movies() {
    const {
        movies, error, isFetching, query,
    } = useAppSelector((state) => state.search);

    if (!error && !isFetching && query && movies.length === 0) {
        return <>No results</>;
    }

    return (
        <Ul>
            {movies
                .map((movie) => {
                    return (<li key={movie.id}><Link to={`/movie/${movie.id}`}>{movie.title}</Link></li>);
                })}
        </Ul>
    );
}
