import React from 'react';
import Rating from '@mui/material/Rating';
import Typography from '@mui/material/Typography';

interface Props {
  rating?: number;
}

export default function RatingsBox({ rating = 0 }: Props) {
  return (
    <>
      <Typography component="legend">
        <strong>Average Rating</strong>
      </Typography>
      <Rating
        name="read-only"
        size="small"
        precision={0.5}
        value={rating}
        readOnly
      />
    </>
  );
}
