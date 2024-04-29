import React from 'react';
import { Grid, Typography } from '@mui/material';
import { StyledGrid, StyledImage } from './card.styled';
import FallBackImg from '../../../assets/images/fallback-image.png';

const Card = ({ imageUrl, title, subtitle, description }) => {
  return (
    <StyledGrid container className="d-flex">
      <Grid item>
        <StyledImage
          src={imageUrl}
          alt="Image"
          onError={({ currentTarget }) => {
            currentTarget.src = FallBackImg;
          }}
        />
      </Grid>
      <Grid item xs={8}>
        <Typography variant="h6">{title}</Typography>
        <Typography variant="subtitle1" color="textSecondary">
          {subtitle}
        </Typography>
        <Typography variant="subtitle1" className="line-clamp-4">
          {description}
        </Typography>
      </Grid>
    </StyledGrid>
  );
};

export default Card;
