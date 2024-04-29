import React from 'react';
import Grid from '@mui/material/Grid';
import Divider from '@mui/material/Divider';
import Typography from '@mui/material/Typography';
import { FeedbackContainerProps } from './feedback-container.interface';

const FeedbackContainer = ({ title, subtitle }: FeedbackContainerProps) => {
  return (
    <Grid container alignItems="center" className="p-t-16">
      <Grid item xs={1} lg={3} />
      <Grid item xs={10} lg={6}>
        <Typography variant="h6" color="textSecondary">
          {title}
        </Typography>
        <Divider />
        {subtitle && <Typography variant="subtitle1">{subtitle}</Typography>}
      </Grid>
      <Grid item xs={1} lg={3} />
    </Grid>
  );
};

export default FeedbackContainer;
