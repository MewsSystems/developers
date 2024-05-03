import { Box, Divider, Stack, Tooltip, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import formatDate from '../../../common/helpers/formateDate';
import { Details } from '../../models/Details';

interface QuickInfoProps {
  readonly details: Details | null;
  readonly title: string;
}

export default function MovieQuickInfo({ details, title }: QuickInfoProps) {
  const { t } = useTranslation();

  return (
    <Stack data-testid="movie-quick-info-section" direction="column" sx={{ mb: 8 }} spacing={1} alignItems="center">
      <Box textAlign="center" maxWidth="calc(100svw - 45rem)">
        <Typography variant="h2">{title}</Typography>
      </Box>
      {details && (
        <Stack
          direction="row"
          divider={<Divider orientation="vertical" flexItem />}
          spacing={2}
          justifyContent="space-evenly">
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

          <Tooltip title={t('movieDetails.originalLanguage')} arrow>
            <Typography variant="subtitle1" color="primary.main">
              {details.original_language.toUpperCase()}
            </Typography>
          </Tooltip>

          <Tooltip title={t('movieDetails.genres')} arrow>
            <Typography variant="subtitle1" color="primary.main">
              {details.genres.length
                ? details.genres.map((genre, index) => (index === 0 ? genre.name : `, ${genre.name}`))
                : t('error.notAvailable')}
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
  );
}
