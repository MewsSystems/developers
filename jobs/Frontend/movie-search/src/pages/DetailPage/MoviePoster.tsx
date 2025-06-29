import { Image } from "../../components/Image/Image";
import { ImageSection } from "./DetailPage.internal";
import { getImageUrl } from "../../utils/movieHelpers";
import ImagePlaceholder from "../../assets/no-image-placeholder.jpg";
import type { MovieDetailsResponse } from "../../api/types";

interface MoviePosterProps {
  movie: MovieDetailsResponse;
}

export const MoviePoster = (props: MoviePosterProps) => {
  return (
    <ImageSection>
      <Image
        src={
          props.movie.poster_path
            ? getImageUrl(props.movie.poster_path)
            : ImagePlaceholder
        }
        alt={
          props.movie.poster_path
            ? `Poster of ${props.movie.original_title}`
            : `No image for ${props.movie.original_title}`
        }
        $width="20rem"
        loading="lazy"
      />
    </ImageSection>
  );
};
