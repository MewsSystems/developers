import React from 'react';
import { Color, FontSize, Spacing } from '../../types';
import Box from '../../elements/Box';
import Text from '../../elements/Text';
import Image from '../../elements/Image';
import Heading from '../../elements/Heading';
import { useSelector } from 'react-redux';
import { selectMovie } from '../../services/appReducer';
import { redirect } from 'react-router-dom'
import { TMDB_IMAGE_BASE_URL } from '../../services/movie';

import { RiMovie2Line } from 'react-icons/ri'
import ImageWithSpinner from './ImageWithSpinner/ImageWithSpinner';
import Rating from './Rating/Rating';

const getYear = (date: string) => {
    return date.split('-')[0];
}

function MovieDetailsPage() {

    const movie = useSelector(selectMovie);
    if (!movie) {
        redirect("/");
        return <></>
    }

    return (
        <Box my={Spacing.big} >
            <Heading size={FontSize.huge}>
                <RiMovie2Line /> {movie.title} <Text size={FontSize.big}>({getYear(movie.release_date)})</Text>
            </Heading>
            <Box style={{border: '2px solid ' + Color['secondary.light']}} mb={Spacing.base}></Box>
            <Box height='60%' style={{ overflow: 'auto' }}>
                <Box pt={Spacing.base} pb={Spacing.big}>
                    <Rating rating={movie.vote_average} count={movie.vote_count} starCount={5} />
                    <Text size={FontSize.small}>{movie.overview}</Text>
                </Box>
                <Box mb={Spacing.huge}>
                    <ImageWithSpinner width={'100%'} alt="poster" src={TMDB_IMAGE_BASE_URL + movie.poster_path}></ImageWithSpinner>
                </Box>
            </Box>
        </Box>
    );
}

export default MovieDetailsPage;
