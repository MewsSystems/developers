import styled from 'styled-components';
import Box from '@/components/Box';

export const MovieBackdrop = styled(Box).withConfig({
  shouldForwardProp: (prop) => prop !== 'backdropPath'
})<{ backdropPath: string }>`
  aspect-ratio: 16/6;
  margin: -24px -24px 0 -24px;
  position: relative;

  &:before {
    display: block;
    content: '';
    width: 100%;
    height: 90px;
    background: linear-gradient(to top, #e9ebf2 50%, rgba(238, 241, 245, 0) 100%);
    z-index: 1;
    position: absolute;
    bottom: 0;
    left: 0;
    right: 0;
  }

  &:after {
    display: block;
    content: '';
    background: url("${process.env.NEXT_PUBLIC_BASE_IMAGE_URL}/original${({backdropPath}) => backdropPath}"), linear-gradient(to top, #09203f 0%, #537895 100%);
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center center;
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    aspect-ratio: 16/6;
    margin: -24px -24px 0 -24px;
    z-index: -1;
  }
`;
