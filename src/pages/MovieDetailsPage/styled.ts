import styled from 'styled-components';

export const MovieDetailsPageContainer = styled.div`
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  padding: 24px;
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
  position: relative;
`;

export const Content = styled.div`
  display: grid;
  grid-template-columns: minmax(300px, 1fr) 2fr;
  gap: 48px;
  margin-top: 24px;

  @media (max-width: 1024px) {
    grid-template-columns: minmax(250px, 1fr) 2fr;
    gap: 32px;
  }

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    gap: 24px;
  }
`;

export const PosterContainer = styled.div`
  position: relative;
  border-radius: 12px;
  overflow: hidden;
  box-shadow:
    0 4px 6px -1px rgb(0 0 0 / 0.1),
    0 2px 4px -2px rgb(0 0 0 / 0.1);
  aspect-ratio: 2/3;
  background: #f3f4f6;

  @media (max-width: 768px) {
    max-width: 300px;
    margin: 0 auto;
  }

  img {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
`;

export const MovieInfo = styled.div`
  display: flex;
  flex-direction: column;
  gap: 24px;
`;

export const Title = styled.h1`
  font-size: 32px;
  font-weight: 600;
  color: #111827;
  margin: 0;

  @media (max-width: 768px) {
    font-size: 28px;
    text-align: center;
  }
`;

export const Overview = styled.p`
  font-size: 16px;
  line-height: 1.6;
  color: #4b5563;
  margin: 0;
`;

export const MetadataInfo = styled.div`
  display: flex;
  gap: 24px;
  flex-wrap: wrap;

  @media (max-width: 768px) {
    justify-content: center;
    gap: 16px;
  }
`;

export const MetadataItem = styled.div`
  display: flex;
  flex-direction: column;
  gap: 4px;

  @media (max-width: 768px) {
    align-items: center;
  }
`;

export const MetadataLabel = styled.span`
  font-size: 14px;
  color: #6b7280;
`;

export const MetadataValue = styled.span`
  font-size: 16px;
  color: #111827;
`;

export const Rating = styled.span<{$rating: number}>`
  padding: 4px 12px;
  border-radius: 6px;
  font-weight: 500;
  color: ${({$rating}) => {
    if ($rating >= 7) return '#059669';
    if ($rating >= 5) return '#d97706';
    return '#dc2626';
  }};
  background: ${({$rating}) => {
    if ($rating >= 7) return '#ecfdf5';
    if ($rating >= 5) return '#fffbeb';
    return '#fef2f2';
  }};
`;

export const LoadingOverlay = styled.div`
  position: absolute;
  inset: 0;
  background: rgba(255, 255, 255, 0.3);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
  backdrop-filter: blur(5px);
`;

export const NotFoundMessageContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: calc(100vh - 100px);
  text-align: center;
  padding: 0 24px;
`;
