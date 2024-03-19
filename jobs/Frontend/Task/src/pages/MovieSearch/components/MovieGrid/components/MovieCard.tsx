import React, { useState } from "react";
import imageNotFound from "../../../../../assets/not_found.svg";
import imagePlaceholder from "../../../../../assets/placeholder.svg";
import { MovieDetailsDrawer } from "../../MovieDetailsDrawer/MovieDetailsDrawer";
import { Movie } from "../../../../../types";

type MovieCardProps = Movie

export const MovieCard = ({ id, path, title }: MovieCardProps) => {
  const [ isImageLoaded, setIsImageLoaded ] = useState(false);
  const [ selectedMovieId, setSelectedMovieId ] = useState<number>();
  const [ isDrawerOpen, setIsDrawerOpen ] = useState(false);

  return (
    <>
      {!isImageLoaded && <img alt={title} src={imagePlaceholder} />}

      <img
        alt={title}
        src={path
          ? `https://image.tmdb.org/t/p/w200/${path}`
          : imageNotFound
        }
        onClick={() => {
          setSelectedMovieId(id);
          setIsDrawerOpen(true);
        }}
        onError={(event) => {
          if ((event.target instanceof HTMLImageElement)) {
            event.target.src = imageNotFound;
          }
        }}
        onLoad={() => setIsImageLoaded(true)}
        style={{ 
          display: isImageLoaded ? "block" : "none",
          cursor: "pointer"
        }}
      />
      <MovieDetailsDrawer
        id={selectedMovieId} 
        isOpen={isDrawerOpen} 
        onClose={() => setIsDrawerOpen(false)}
      />
    </>
  );
};
