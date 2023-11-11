import styled from "styled-components";
import fallbackAvatar from "@/assets/mocks/avatar-fallback.png";
import { MOVIE_POSTER_SMALL_BASE_PATH } from "@/pages/Search";
import { Typography } from "..";

export interface AvatarProps {
  imgPath?: string;
  name?: string;
  description?: string;
}

const AvatarWrapper = styled.div`
  display: flex;
  flex-direction: column;
  text-align: center;
`;

const AvatarImage = styled.img`
  width: 120px;
  height: 120px;

  object-fit: cover;
  border-radius: 100px;
`;

export function Avatar({ imgPath, name, description }: AvatarProps) {
  return (
    <AvatarWrapper>
      {/* TODO: replace with avatar own const path */}
      <AvatarImage src={imgPath ? MOVIE_POSTER_SMALL_BASE_PATH + imgPath : fallbackAvatar} />
      {name && <Typography>{name}</Typography>}
      {description && <Typography variant="labelMedium">{description}</Typography>}
    </AvatarWrapper>
  );
}
