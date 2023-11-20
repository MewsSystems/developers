import { FC } from 'react';
import { Color, FontSize, Movie, Spacing } from '../../../types';
import Text from '../../../elements/Text';
import Box from '../../../elements/Box';
import { Link } from 'react-router-dom';
import { AppDispatch } from '../../../store';
import { useDispatch } from 'react-redux';
import { setMovie } from '../../../services/appReducer';
import { RiMovie2Line } from 'react-icons/ri'
import Rating from '../../MovieDetailsPage/Rating/Rating';


const ResultLink: FC<{ movie: Movie }> = ({ movie }) => {

    const dispatch = useDispatch<AppDispatch>();

    const onMovieClick = (movie: Movie) => {
        dispatch(setMovie(movie));
    }

    const border = '1px dashed ' + Color['secondary.dark'];

    return (
        <Link style={{ textDecoration: 'none' }} to={"/movie"} onClick={() => onMovieClick(movie)}>
            <Box p={Spacing.small} py={Spacing.base} style={{ borderBottom: border }}>
                <Box inline style={{ float: 'right' }}>
                    <Rating starCount={5} rating={movie.vote_average}/>
                </Box>
                <Box width='calc(100% - 100px)'>
                    <Text color={Color.link} size={FontSize.small}>
                        {movie.title}
                    </Text>
                </Box>
            </Box>
        </Link>
    )
}

export default ResultLink;