import React from "react";
import { LazyLoadImage } from "react-lazy-load-image-component";
import {
  PropertyList,
  PropertyTitle,
  PropertyValue,
} from "../PropertyList";
import {
  MovieDetails,
  MovieDetailWrapper,
  MovieTitle,
  PosterWrapper,
  SectionTitle,
  Tagline,
  TextParagraph,
} from "./MovieDetailTitle.styled";

interface MovieDetailProps {
  title: string;
  posterUrl?: string;
  backdropUrl?: string;
  tagline?: string;
  overview?: string;
  releaseDate?: string;
  genres?: string[];
  runtime?: number;
}

export const MovieDetailTitle = (props: MovieDetailProps) => {
  const { 
    title,
    backdropUrl,
    posterUrl,
    tagline,
    overview,
    releaseDate,
    genres,
    runtime,
  } = props;

  return (
    <MovieDetailWrapper backdropUrl={backdropUrl}>
      {posterUrl ? <PosterWrapper>
        <LazyLoadImage
          alt={title}
          src={posterUrl}
          width="100%"
          effect="opacity"
        />
      </PosterWrapper> : null}
      <MovieDetails>
        <MovieTitle>{title}</MovieTitle>
        {tagline && <Tagline>{tagline}</Tagline>}
        {overview && <>
          <SectionTitle>Overview</SectionTitle>
          <TextParagraph>
            {overview}
          </TextParagraph>
        </>}
        {(releaseDate || genres || runtime) && <>
          <SectionTitle>General info</SectionTitle>
          <PropertyList textColor="#fff" sideBySide={true}>
            {releaseDate && <>
              <PropertyTitle>Release date</PropertyTitle>
              <PropertyValue>{releaseDate}</PropertyValue>
            </>}
            {genres && genres.length > 0 && <>
              <PropertyTitle>Genres</PropertyTitle>
              <PropertyValue>{genres.join(', ')}</PropertyValue>
            </>}
            {runtime && <>
              <PropertyTitle>Runtime</PropertyTitle>
              <PropertyValue>{runtime} minutes</PropertyValue>
            </>}
          </PropertyList>
        </>}
      </MovieDetails>
    </MovieDetailWrapper>
  );
};
