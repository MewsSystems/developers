import { Box, Divider, Paper, Stack, SxProps, Theme, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import { Details } from '../models/Details';

interface ReceptionProps {
  readonly details: Details;
  readonly sxPaperContainer: SxProps<Theme>;
}

export default function MovieReception({ details, sxPaperContainer }: ReceptionProps) {
  const { t } = useTranslation();

  return (
    <Paper sx={sxPaperContainer} elevation={3}>
      <Stack direction="column" spacing={1}>
        <Box>
          <Typography variant="h5" color="primary.main">
            {t('movieDetails.reception')}
          </Typography>
        </Box>

        <Stack
          direction="row"
          divider={<Divider orientation="vertical" flexItem />}
          spacing={2}
          justifyContent="space-evenly">
          <Box>
            <Typography sx={{ fontWeight: 400 }} variant="h6" display="inline" color="secondary.main">
              {`${t('movieDetails.voteAverage')}: `}
            </Typography>
            <Typography variant="h5" display="inline" color="primary.main">
              {`${Math.round(details.vote_average)}/10`}
            </Typography>
          </Box>

          <Box>
            <Typography sx={{ fontWeight: 400 }} variant="h6" display="inline" color="secondary.main">
              {`${t('movieDetails.voteCount')}: `}
            </Typography>
            <Typography variant="h5" display="inline" color="primary.main">
              {details.vote_count}
            </Typography>
          </Box>
        </Stack>
      </Stack>
    </Paper>
  );
}
