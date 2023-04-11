import styled from "styled-components";
import { BASE_IMG_URL } from "../../config/api";
import { colors, device } from "../../utils/theme";

const MovieImgContainer = styled.img`
  width: 9em;
  height: 13.5em;
  border-radius: 10px;
  @media ${device.mobileL} {
    width: 11em;
    height: 16.5em;
  }
`;

const MovieName = styled.div`
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 9em;
  color: ${colors.primaryText};
  @media ${device.mobileL} {
    max-width: 11em;
  }
`;

interface MovieImgProps {
  banner: string;
  movieName: string;
}

/**
 * Movie banner and name container
 * @param props {banner, movieName} url of the image and movie name
 * @returns renders the movie banner with movie name
 */
const MovieImg = (props: MovieImgProps) => {
  const { banner, movieName } = props;
  const imageUrl = `${BASE_IMG_URL}${banner}`;
  return (
    <div>
      <MovieImgContainer alt={movieName} src={imageUrl}></MovieImgContainer>
      <MovieName>{movieName}</MovieName>
    </div>
  );
};

export default MovieImg;
