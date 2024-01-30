import styled from "styled-components";

export const StyledMovieDetailModalHeader = styled.div`
  width: 100%;
  display: flex;
  justify-content: flex-end;
`;

export const StyledMovieDetailModalContainer = styled.div`
  display: flex;
  justify-content: center;
  padding: 0 1rem 1rem;

  @media (max-width: 768px) {
    flex-direction: column;
    align-items: center;
  }
`;

export const StyledMovieDetailModalImage = styled.img`
  width: 15rem;
  height: auto;
  max-height: 25rem;
  border-radius: 0.5rem;
  transition: transform 0.2s ease;
  margin: 0 1rem 1rem;
`;

export const StyledMovieDetailModalWrapper = styled.div`
  display: flex;
  flex-direction: column;
  margin: 0 1rem;
`;

export const StyledMovieDetailModalHeadline = styled.h2`
  display: -webkit-box;
  -webkit-box-orient: vertical;
  overflow: hidden;
  -webkit-line-clamp: 2;
  text-overflow: ellipsis;
  font-size: 1.5rem;
  font-weight: 600;
  margin: 0;
`;

export const StyledMovieDetailModalParagraph = styled.p`
  font-size: 1rem;
  font-weight: 400;
  margin: 0.8rem 0 0 0;
  overflow: hidden;
  max-height: 200px;
  overflow-y: auto;
`;
