import { Box, Paper, Stack, SxProps, Theme, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { Details } from '../models/Details';

interface OverviewProps {
  readonly details: Details | null;
  readonly sxPaperContainer: SxProps<Theme>;
}

export default function MovieOverview({ details, sxPaperContainer }: OverviewProps) {
  const { t } = useTranslation();

  return (
    <Paper sx={sxPaperContainer} elevation={3}>
      <Stack direction="column" spacing={2}>
        <Box>
          <Typography variant="h5" color="primary.main">
            {t('movieDetails.overview')}
          </Typography>
          <Typography variant="body1">{details?.overview}</Typography>
        </Box>

        <Box textAlign="center">
          {details?.tagline && (
            <Typography sx={{ fontStyle: 'italic', fontWeight: 'lighter' }} variant="h6" color="primary.main">
              {`"${details.tagline}"`}
            </Typography>
          )}
        </Box>
      </Stack>
    </Paper>
  );
}
