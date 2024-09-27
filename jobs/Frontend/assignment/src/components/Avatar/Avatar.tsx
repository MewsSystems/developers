import styled from "styled-components";
import fallbackAvatar from "@/assets/mocks/avatar-fallback.png";
import { Typography } from "..";
import { MEDIA_185_BASE_URL } from "@/tmdbClient";

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
      <AvatarImage src={imgPath ? MEDIA_185_BASE_URL + imgPath : fallbackAvatar} />
      {name && <Typography>{name}</Typography>}
      {description && <Typography variant="labelMedium">{description}</Typography>}
    </AvatarWrapper>
  );
}
