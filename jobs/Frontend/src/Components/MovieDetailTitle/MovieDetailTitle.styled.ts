import styled from "styled-components";

interface MovieDetailWrapperProps {
  backdropUrl?: string;
}

export const MovieDetailWrapper = styled.div<MovieDetailWrapperProps>`
  width: 100%;
  box-sizing: border-box;
  background-color: #000;
  position: relative;
  padding: 1rem;
  overflow: hidden;
  display: flex;
  flex-direction: row;
  gap: 1rem;

  & * {
    position: relative;
    z-index: 10;
  }

  &::after {
    content: '';
    position: absolute; 
    background-image: ${p => p.backdropUrl ? `url('${p.backdropUrl}')` : 'none'};
    background-size: cover;
    opacity: 0.25;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    z-index: 1;
    filter: blur(5px);
  }

  & img {
    width: 100%;
  }
`;

export const PosterWrapper = styled.div`
  flex-basis: 200px;
  @media (max-width: 900px) {
    flex-basis: 150px;
  }
`;

export const MovieDetails = styled.div`
  color: white;
  line-height: 2rem;
  flex: 1;
`;

export const MovieTitle = styled.h1`
  font-size: 2.5rem;
  font-weight: bold;
`;

export const Tagline = styled.h2`
  font-size: 1.6rem;
  font-weight: normal;
  color: #ddd;
`;

export const SectionTitle = styled.h3`
  font-size: 1.8rem;
  font-weight: bold;
  margin-top: 2.0rem;
`;

export const TextParagraph = styled.p`
  font-size: 1.6rem;
  line-height: 1.9rem;
  color: white;
`;
