import React from "react";
import { LazyLoadImage } from "react-lazy-load-image-component";
import {
  MoviePreviewLayout,
  MoviePreviewTitle,
  NoPoster,
  PosterPlaceholder,
} from "./MoviePreview.styled";

interface MoviePreviewProps {
  posterUrl?: string;
  title: string;
}

export const MoviePreview = (props: MoviePreviewProps) => {
  return (
    <MoviePreviewLayout>
      <PosterPlaceholder>
        {props.posterUrl ? <LazyLoadImage
          alt={props.title}
          src={props.posterUrl}
          placeholderSrc={PosterPlaceholder}
          width={200}
        /> : 
          <NoPoster>No poster</NoPoster>
        }
      </PosterPlaceholder>
      <MoviePreviewTitle>{props.title}</MoviePreviewTitle>
    </MoviePreviewLayout>
  )
};
