import styled from 'styled-components';
import Box from '@/components/Box';

export interface MoviePosterProps {
  posterUrl?: string;
}

export const MoviePoster = styled(Box)<MoviePosterProps>`
  background-image: url("${({posterUrl}) => `${process.env.NEXT_PUBLIC_BASE_IMAGE_URL}/w300${posterUrl}`}"), linear-gradient(to top, #09203f 0%, #537895 100%);
  background-repeat: no-repeat;
  background-size: cover;
  background-position: bottom center;
  width: 200px;
  height: auto;
  flex-grow: 1;
`;
