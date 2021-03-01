import styled from 'styled-components';

interface MoviePosterSkeletonProps {
  height?: string;
  width?: string;
}

export const MoviePosterSkeleton = styled.div<MoviePosterSkeletonProps>`
  background-color: gray;
  height: ${(props) => props.height || '100%'};
  width: ${(props) => props.width || '100%'};

  > img {
    width: 100%;
    height: 100%;
  }
`;
