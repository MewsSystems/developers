import React from 'react';
import {Link} from 'react-router-dom';
import {Button, Grid, Typography} from '@mui/material';
import {useTranslation} from 'react-i18next';

const NotFound = () => {
  const {t} = useTranslation();
  return (
    <Grid container direction="column" spacing={2} mt={1} justifyContent="center" alignItems="center">
      <Grid item style={{textAlign: 'center'}}>
        <Typography variant="h3" component="div">
          {t('error.page_not_found')}
        </Typography>
      </Grid>
      <Grid item>
        <Button component={Link} to={'/'} color="primary" variant="contained" sx={{boxShadow: 0}}>
          {t('error.go_home')}
        </Button>
      </Grid>
    </Grid>
  );
};

export default NotFound;
