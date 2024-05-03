import LinkIcon from '@mui/icons-material/Link';
import { Box, Link, Paper, Stack, SxProps, Theme, Tooltip, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';
import formatCurrency from '../../../common/helpers/formatCurrency';
import { Details } from '../../models/Details';

interface AdditionalInformationProps {
  readonly details: Details;
  readonly sxPaperContainer: SxProps<Theme>;
}

export default function MovieAdditionalInformation({ details, sxPaperContainer }: AdditionalInformationProps) {
  const { t } = useTranslation();

  return (
    <Paper data-testid="movie-additional-information-section" sx={sxPaperContainer} elevation={3}>
      <Stack direction="column" spacing={1}>
        <Box>
          <Typography variant="h5" color="primary.main">
            {t('movieDetails.additionalInfo')}
          </Typography>
        </Box>

        <Stack direction="column" alignItems="left" justifyContent="space-between">
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

          <Box>
            <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
              {`${t('movieDetails.spokenLanguages')}: `}
            </Typography>
            <Typography variant="subtitle1" display="inline">
              {details.spoken_languages.map((language, index) => (index === 0 ? language.name : `, ${language.name}`))}
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
              {details.budget ? formatCurrency(details.budget) : t('error.notAvailable')}
            </Typography>
          </Box>

          <Box>
            <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
              {`${t('movieDetails.revenue')}: `}
            </Typography>
            <Typography variant="subtitle1" display="inline">
              {details.revenue ? formatCurrency(details.revenue) : t('error.notAvailable')}
            </Typography>
          </Box>

          <Box>
            <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
              {`${t('movieDetails.productionCompanies')}: `}
            </Typography>

            <Typography variant="subtitle1" display="inline">
              {details.production_companies.length
                ? details.production_companies.map((company, index) =>
                    index === 0 ? company.name : `, ${company.name}`
                  )
                : t('error.notAvailable')}
            </Typography>
          </Box>

          <Box>
            <Typography sx={{ fontWeight: 'bold' }} variant="subtitle1" display="inline">
              {`${t('movieDetails.productionCountries')}: `}
            </Typography>
            <Typography variant="subtitle1" display="inline">
              {details.production_countries.length
                ? details.production_countries.map((country, index) =>
                    index === 0 ? country.name : `, ${country.name}`
                  )
                : t('error.notAvailable')}
            </Typography>
          </Box>
        </Stack>
      </Stack>
    </Paper>
  );
}
