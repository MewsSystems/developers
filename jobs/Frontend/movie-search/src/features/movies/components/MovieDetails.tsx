import { Box, CircularProgress, Stack, SxProps, Theme, Typography } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate, useParams } from 'react-router-dom';
import AppRoutes from '../../../configs/appRoutes';
import { ENDPOINT_URL_IMAGES_w500 } from '../../../configs/config';
import { getMovieDetails } from '../../api/getMovieDetails';
import { Details } from '../models/Details';
import MovieAdditionalInformation from './MovieAdditionalInfomation';
import MovieOverview from './MovieOverview';
import MovieQuickInfo from './MovieQuickInfo';
import MovieReception from './MovieReception';

export default function MovieDetails() {
  const { movieId } = useParams();
  const { t } = useTranslation();
  const navigate = useNavigate();

  const { isLoading, data, error } = useQuery({
    queryKey: ['details', movieId],
    queryFn: async () =>
      getMovieDetails(movieId as string)
        .then(response => response.data)
        .catch((error: AxiosError) => {
          console.error(error.toJSON());

          if (error.response?.status === 404) {
            navigate(AppRoutes.PageNotFound, { replace: true });
          } else {
            throw new Error(t('error.failedMoviesFetch'));
          }
        })
  });

  const [details, setDetails] = useState<Details | null>(null);
  const [title, setTitle] = useState('');

  useEffect(() => {
    if (data) {
      setDetails(data);
      setTitle(`${data.title} (${data.release_date.substring(0, 4)})`);
    }
  }, [data, t]);

  useEffect(() => {
    document.title = t('movieDetails.appTitle', { movieTitle: title });
  }, [t, title]);

  const sxPaperContainer: SxProps<Theme> = { p: 4, borderRadius: 2, maxWidth: '70rem', height: 'fit-content' };

  return (
    <>
      <MovieQuickInfo details={details} title={title} />

      {isLoading && <CircularProgress sx={{ m: 4 }} />}

      <Box sx={{ mt: 4 }}>
        {error !== null && (
          <Typography sx={{ mt: 2 }} variant="h5" color="error">
            Error: {error?.message}
          </Typography>
        )}

        {details && (
          <Stack direction="row" spacing={10} alignItems="center">
            <Box>
              <img
                height={750}
                width={500}
                src={ENDPOINT_URL_IMAGES_w500 + details.poster_path}
                alt={`${title} ${t('movieDetails.poster')}`}
              />
            </Box>

            <Stack direction="column" spacing={4}>
              <MovieOverview details={details} sxPaperContainer={sxPaperContainer} />
              <MovieReception details={details} sxPaperContainer={sxPaperContainer} />
              <MovieAdditionalInformation details={details} sxPaperContainer={sxPaperContainer} />
            </Stack>
          </Stack>
        )}
      </Box>
    </>
  );
}
