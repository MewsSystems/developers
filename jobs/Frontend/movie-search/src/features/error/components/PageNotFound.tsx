import { Stack, Typography } from '@mui/material';

export default function PageNotFound() {
  return (
    <Stack data-testid="page-not-found-content" direction="column" alignItems="center" spacing={4}>
      <Typography variant="h1">{'(╯°□°)╯︵ ┻━┻'}</Typography>
      <Typography sx={{ fontWeight: 'bold' }} variant="h2" color="error.main">
        404
      </Typography>
      <Typography variant="h3">Something went wrong! This page could not be found.</Typography>
    </Stack>
  );
}
