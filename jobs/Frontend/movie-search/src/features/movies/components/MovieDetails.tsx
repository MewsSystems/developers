import { Box, CircularProgress, Divider, Paper, Stack, Typography } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useParams } from 'react-router-dom';
import { ENDPOINT_URL_IMAGES_w500 } from '../../../configs/config';
import { getMovieDetails } from '../../api/getMovieDetails';
import formatDate from '../../common/helpers/formateDate';
import { Details } from '../models/Details';

export default function MovieDetails() {
  const { movieId } = useParams();
  const { t } = useTranslation();

  const { isLoading, data, error } = useQuery({
    queryKey: ['details', movieId],
    queryFn: async () =>
      getMovieDetails(movieId as string)
        .then(response => response.data)
        .catch((error: AxiosError) => {
          console.error(error.toJSON());
          throw new Error(t('error.failedMoviesFetch'));
        })
  });

  const [details, setDetails] = useState<Details | null>(null);
  const [title, setTitle] = useState('');
  const [adultRating, setAdultRating] = useState('');

  useEffect(() => {
    if (data) {
      setDetails(data);
      setTitle(`${data.title} (${data.release_date.substring(0, 4)})`);
      setAdultRating(`${data?.adult ? t('common.yes') : t('common.no')}`);
    }
  }, [data, t]);

  useEffect(() => {
    document.title = t('movieDetails.appTitle', { movieTitle: title });
  }, [t, title]);

  return (
    <>
      <Box sx={{ mb: 8 }}>
        <Typography variant="h2">{title}</Typography>
        {details && (
          <Stack direction="row" divider={<Divider orientation="vertical" flexItem />} spacing={2}>
            <Typography variant="subtitle1" color="primary.main">
              {`${t('movieDetails.status')}: ${details.status}`}
            </Typography>

            <Typography variant="subtitle1" color="primary.main">
              {formatDate(details.release_date)}
            </Typography>

            <Typography variant="subtitle1" color="primary.main">
              {`${t('movieDetails.adult')}: ${adultRating}`}
            </Typography>

            <Typography variant="subtitle1" color="primary.main">
              {details.original_language.toUpperCase()}
            </Typography>

            <Typography variant="subtitle1" color="primary.main">
              {details.genres.map((genre, index) => (index === 0 ? genre.name : `, ${genre.name}`))}
            </Typography>
          </Stack>
        )}
      </Box>

      {isLoading && <CircularProgress sx={{ m: 4 }} />}

      <Box sx={{ mt: 4 }}>
        {error !== null && (
          <Typography sx={{ mt: 2 }} variant="h5" color="error">
            Error: {error?.message}
          </Typography>
        )}

        {details && (
          <Stack direction="row" spacing={10}>
            <Box>
              <img
                height={750}
                width={500}
                src={ENDPOINT_URL_IMAGES_w500 + details.poster_path}
                alt={`${title} ${t('movieDetails.poster')}`}
              />
            </Box>

            <Paper sx={{ p: 4, borderRadius: 2, maxWidth: '70rem', height: 'fit-content' }} elevation={3}>
              <Typography variant="h5" color="primary.main">
                {t('movieDetails.overview')}
              </Typography>
              <Typography variant="body1">{details.overview}</Typography>
            </Paper>
          </Stack>
        )}
      </Box>
    </>
  );
}
