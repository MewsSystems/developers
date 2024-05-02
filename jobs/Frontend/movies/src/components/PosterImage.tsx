import { PosterSize, posterUrl } from "src/api/tmdb";
import styled from "styled-components";
import FilmIcon from "src/assets/film.svg?component";

type PosterImageProps = {
  path: string | undefined;
  size: PosterSize;
};

function PosterImage({ path, size }: PosterImageProps) {
  return path ? (
    <Image src={posterUrl(path, size)} alt="poster" role="presentation" />
  ) : (
    <EmptyImage />
  );
}

const Image = styled.img`
  width: 100%;
  height: 100%;
  border-radius: 12px;
`;

const EmptyImage = styled(FilmIcon)`
  width: 100%;
  height: 100%;
  border-radius: 12px;
  margin: auto;
  color: white;
  background-color: var(--color-light-gray);
`;

export default PosterImage;
