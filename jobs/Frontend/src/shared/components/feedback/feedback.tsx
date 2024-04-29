import * as React from 'react';
import { Container, Typography } from '@mui/material';

const Feedback = ({ title, subtitle }) => {
  return (
    <Container maxWidth="sm" style={{ marginTop: '32px' }}>
      <Typography variant="h4" gutterBottom>
        {title}
      </Typography>
      <Typography variant="body1">{subtitle}</Typography>
    </Container>
  );
};

export default Feedback;
