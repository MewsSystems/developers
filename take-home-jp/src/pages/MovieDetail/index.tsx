import React, { useEffect } from 'react';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import useGetMovieById from '../../api/useGetMovieById';
import { useParams } from 'react-router';
import Button from '../../components/Button';
import Box from '@mui/system/Box/Box';
import { displayMovieRunTime, formatFullYearDate } from './utils';
import { IMAGE_URL } from '../../api/endpoints';
import { styles } from './styles';
import RatingsBox from '../../components/RatingsBox';
import ChipRow from '../../components/ChipRow';

const MoveDetail = () => {
  const { data: selectedMovie, get: getMovieById } = useGetMovieById();
  const { id: movieId } = useParams();
  useEffect(() => {
    if (movieId) {
      getMovieById(movieId);
    }
  }, []);
  return (
    <div
      style={{
        ...styles.container,
        backgroundImage: `url('${IMAGE_URL + selectedMovie?.backdrop_path}')`,
      }}
    >
      <div style={styles.container2}>
        <Container style={styles.container3}>
          <>
            <Box style={styles.container4} mb={6}>
              <Typography variant="h3" component="h1" style={styles.movieTitle}>
                {selectedMovie?.title}
              </Typography>
              <Box style={styles.ratingsContainer}>
                <RatingsBox
                  rating={
                    selectedMovie?.vote_average
                      ? selectedMovie.vote_average / 2
                      : 0
                  }
                />
              </Box>
            </Box>
            <Typography component="p" mb={2} style={styles.infoText}>
              {`${formatFullYearDate(selectedMovie?.release_date)} | ${displayMovieRunTime(selectedMovie?.runtime)} `}
            </Typography>
            <Box mb={5}>
              <ChipRow collection={selectedMovie?.genres || []} />
            </Box>
            <Typography component="p" mb={6}>
              {selectedMovie?.overview}
            </Typography>
            <Box mb={2}>
              <Button>Learn More</Button>
            </Box>
          </>
        </Container>
      </div>
    </div>
  );
};

export default MoveDetail;
