import { hasString } from "../../utils/public-api";
import { ImageStyled } from "./Image.styled";

const MOVIE_IMG_URL = "https://image.tmdb.org/t/p/w185";

type Props = {
  src: string | null;
  alt: string;
  size: number;
};

export const Image = ({ src, alt, size }: Props) => {
  return (
    <ImageStyled
      src={src ? `${MOVIE_IMG_URL}${src}` : "placeholder.png"}
      alt={hasString(alt) ? alt : "Placeholder Image"}
      width={size}
    />
  );
};
