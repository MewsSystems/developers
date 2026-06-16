import React from "react";
import Box from "@mui/material/Box";
import Chip from "@mui/material/Chip";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import { useQueryMovieDetails } from "../../../../../hooks/api/useQueryMovieDetails/useQueryMovieDetails";
import { EmptyState } from "../../../../../components/EmptyState/EmptyState";
import { LoadingState } from "../../../../../components/LoadingState/LoadingState";

type MovieDetailsProp = {
  id: number;
}

export const MovieDetails = ({ id }: MovieDetailsProp) => {
  const { data, isLoading, isError } = useQueryMovieDetails({ id });

  if (isLoading) {
    return <LoadingState />;
  }

  if (isError) {
    return (
      <EmptyState 
        title="Something went wrong"
        description="Please try again later"
      />
    );
  }

  if (!data) {
    return (
      <EmptyState 
        title="No data was found"
        description="It seems there is no data available for this movie title"
      />
    );
  }

  return (
    <>
      <Typography variant='h5' paddingBottom={1}>
        <Box fontWeight={600}>
          {data.title}
        </Box>
      </Typography>

      <Typography variant='body2' paddingBottom={2}>
        Release date: {data.release_date || "unknown"}
      </Typography>

      {data.genres && data.genres.length > 0 && (
        <Stack direction="row" spacing={1} mb={2}>
          {data.genres.map(genre => 
            <Chip key={genre.name} label={genre.name} />
          )}
        </Stack>
      )}
      
      <Typography>
        {data.overview}
      </Typography>
    </>
  );
};
