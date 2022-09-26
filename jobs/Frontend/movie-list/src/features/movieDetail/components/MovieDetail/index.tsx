import Head from "next/head";
import Image, { ImageLoader } from "next/image";
import { FC } from "react";

import { useGetMovieIdQuery } from "../../hooks";
import { MovieDetailDescription } from "./parts/MovieDetailDescription";
import { ImageContainer, MovieDetailContainer } from "./styled";

import fallBackImg from "~/features/ui/assets/video-movie-placeholder-image-grey.png";

type Props = {
  id: number;
};

const movieImageLoader =
  (hasOriginalSource: Boolean): ImageLoader =>
  ({ src }) => {
    if (!hasOriginalSource) {
      return src;
    }

    return `${process.env.NEXT_PUBLIC_IMAGE_PATH}${src.slice(1)}`;
  };

export const MovieDetail: FC<Props> = ({ id }) => {
  const { data, isLoading, isError } = useGetMovieIdQuery(id);

  if (isLoading) {
    return <p>Page is loading</p>;
  }

  if (isError || !data) {
    return <p>Could not find movie</p>;
  }

  const { poster_path, original_title } = data;

  return (
    <>
      <Head>
        <title>{`${
          data.original_title || "Movie title"
        } | The Movie List`}</title>
      </Head>
      <MovieDetailContainer>
        <ImageContainer>
          <Image
            src={poster_path || fallBackImg}
            loader={movieImageLoader(!!poster_path)}
            alt={`${original_title} - Poster Image`}
            height={600}
            width={400}
            layout="responsive"
            style={{ borderRadius: "7px" }}
          />
        </ImageContainer>
        <MovieDetailDescription movieDetailDescription={data} />
      </MovieDetailContainer>
    </>
  );
};
