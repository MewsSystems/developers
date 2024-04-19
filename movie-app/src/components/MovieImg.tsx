import { HTMLAttributes } from "react";
import { Movie } from "../hooks/movies/useSearchMovie.ts";
import { Box } from "@mui/material";

type MovieImgProps = Pick<Movie, "title"> & {
  posterPath?: string | null;
} & HTMLAttributes<HTMLImageElement>;

const MovieImg = (props: MovieImgProps) => {
  const { posterPath, title, ...imgProps } = props;

  return (
    <Box
      aria-hidden
      sx={{
        flexShrink: 0,
        height: 200,
        width: 300,
        borderRadius: 5,
        backgroundRepeat: "no-repeat",
        backgroundSize: "cover",
        backgroundImage: posterPath
          ? `url('https://media.themoviedb.org/t/p/w220_and_h330_face${posterPath}')`
          : "linear-gradient(to right bottom, #02011D, #474860)",
      }}
      {...imgProps}
    />
  );
};

export default MovieImg;
