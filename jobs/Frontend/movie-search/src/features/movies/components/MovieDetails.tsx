import LinkIcon from '@mui/icons-material/Link';
import { Box, CircularProgress, Divider, Link, Paper, Stack, SxProps, Theme, Tooltip, Typography } from '@mui/material';
import { useQuery } from '@tanstack/react-query';
import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useParams } from 'react-router-dom';
import { ENDPOINT_URL_IMAGES_w500 } from '../../../configs/config';
import { getMovieDetails } from '../../api/getMovieDetails';
import formatCurrency from '../../common/helpers/formatCurrency';
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
      <Stack direction="column" sx={{ mb: 8 }} spacing={1}>
        <Box textAlign="center">
          <Typography variant="h2">{title}</Typography>
        </Box>
        {details && (
          <Stack
            direction="row"
            divider={<Divider orientation="vertical" flexItem />}
            spacing={2}
            justifyContent="space-between">
            <Tooltip title={t('movieDetails.status')} arrow>
              <Typography variant="subtitle1" color="primary.main">
                {details.status}
              </Typography>
            </Tooltip>
            <Tooltip title={t('movieDetails.releaseDate')} arrow>
              <Typography variant="subtitle1" color="primary.main">
                {formatDate(details.release_date)}
              </Typography>
            </Tooltip>
            <Typography variant="subtitle1" color="primary.main">
              {`${t('movieDetails.adult')}: ${data?.adult ? t('common.yes') : t('common.no')}`}
            </Typography>
            <Tooltip title={t('movieDetails.originalLanguage')} arrow>
              <Typography variant="subtitle1" color="primary.main">
                {details.original_language.toUpperCase()}
              </Typography>
            </Tooltip>

            <Tooltip title={t('movieDetails.genres')} arrow>
              <Typography variant="subtitle1" color="primary.main">
                {details.genres.map((genre, index) => (index === 0 ? genre.name : `, ${genre.name}`))}
              </Typography>
            </Tooltip>

            <Tooltip title={t('movieDetails.runtime')} arrow>
              <Typography variant="subtitle1" color="primary.main">
                {`${details.runtime} minutes`}
              </Typography>
            </Tooltip>
          </Stack>
        )}
      </Stack>

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

            <Stack direction="column" spacing={4}>
              <Paper sx={sxPaperContainer} elevation={3}>
                <Stack direction="column" spacing={2}>
                  <Box>
                    <Typography variant="h5" color="primary.main">
                      {t('movieDetails.overview')}
                    </Typography>
                    <Typography variant="body1">{details.overview}</Typography>
                  </Box>

                  <Box textAlign="center">
                    <Typography sx={{ fontStyle: 'italic' }} variant="subtitle1" color="primary.main">
                      {`"${details.tagline}"`}
                    </Typography>
                  </Box>
                </Stack>
              </Paper>

              <Paper sx={sxPaperContainer} elevation={3}>
                <Stack direction="column" spacing={1}>
                  <Box>
                    <Typography variant="h5" color="primary.main">
                      {t('movieDetails.additionalInfo')}
                    </Typography>
                  </Box>

                  <Stack direction="column" alignItems="left" justifyContent="space-between">
                    <Box>
                      <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                        {`${t('movieDetails.spokenLanguages')}: `}
                      </Typography>
                      <Typography variant="subtitle1" display="inline">
                        {details.spoken_languages.map((language, index) =>
                          index === 0 ? language.name : `, ${language.name}`
                        )}
                      </Typography>
                    </Box>

                    <Box>
                      <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                        {`${t('movieDetails.originalTitle')}: `}
                      </Typography>
                      <Typography variant="subtitle1" display="inline">
                        {details.original_title}
                      </Typography>
                    </Box>

                    <Box>
                      <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                        {`${t('movieDetails.originCountry')}: `}
                      </Typography>
                      <Typography variant="subtitle1" display="inline">
                        {details.origin_country.map((country, index) => (index === 0 ? country : `, ${country}`))}
                      </Typography>
                    </Box>

                    {details.homepage && (
                      <Box>
                        <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                          {`${t('movieDetails.hompage')}: `}
                        </Typography>
                        <Typography variant="subtitle1" display="inline">
                          <Tooltip title={details.homepage} arrow>
                            <Link href={details.homepage}>{<LinkIcon sx={{ my: -1 }}></LinkIcon>}</Link>
                          </Tooltip>
                        </Typography>
                      </Box>
                    )}
                  </Stack>

                  <Stack direction="column" alignItems="left" justifyContent="space-between">
                    <Box>
                      <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                        {`${t('movieDetails.budget')}: `}
                      </Typography>
                      <Typography variant="subtitle1" display="inline">
                        {formatCurrency(details.budget)}
                      </Typography>
                    </Box>

                    <Box>
                      <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                        {`${t('movieDetails.revenue')}: `}
                      </Typography>
                      <Typography variant="subtitle1" display="inline">
                        {formatCurrency(details.revenue)}
                      </Typography>
                    </Box>

                    <Box>
                      <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                        {`${t('movieDetails.productionCompanies')}: `}
                      </Typography>
                      <Typography variant="subtitle1" display="inline">
                        {details.production_companies.map((company, index) =>
                          index === 0 ? company.name : `, ${company.name}`
                        )}
                      </Typography>
                    </Box>

                    <Box>
                      <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
                        {`${t('movieDetails.productionCountries')}: `}
                      </Typography>
                      <Typography variant="subtitle1" display="inline">
                        {details.production_countries.map((country, index) =>
                          index === 0 ? country.name : `, ${country.name}`
                        )}
                      </Typography>
                    </Box>
                  </Stack>
                </Stack>
              </Paper>
            </Stack>
          </Stack>
        )}
      </Box>
    </>
  );
}
