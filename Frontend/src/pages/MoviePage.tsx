import React from 'react';
import { useParams, useHistory } from 'react-router-dom';
import styled from 'styled-components';
import DetailResult from '../modules/movie-detail/MovieDetailFetcher';

const BackButton = styled.a`
    display: block;
    text-align: center;
    padding: 5px;
`;

export default function MoviePage() {
    const { id } = useParams();
    const { goBack } = useHistory();
    id as string;

    return (
        <>
            <DetailResult
                id={Number(id)}
            />
            <BackButton
                href={''}
                onClick={(e) => { e.preventDefault(); goBack(); }}
            >
                Back to search
            </BackButton>
        </>
    );
}
