import styled from 'styled-components';

export const MovieDetailsPageContainer = styled.div`
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  padding: 24px;
  max-width: 1200px;
  margin: 0 auto;
  width: 100%;
`;

export const Content = styled.div`
  display: grid;
  grid-template-columns: minmax(300px, 2fr) 3fr;
  gap: 48px;
  align-items: start;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
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
`;

export const MetadataItem = styled.div`
  display: flex;
  flex-direction: column;
  gap: 4px;
`;

export const MetadataLabel = styled.span`
  font-size: 14px;
  color: #6b7280;
`;

export const MetadataValue = styled.span`
  font-size: 16px;
  color: #111827;
  font-weight: 500;
`;

export const Rating = styled.div<{$rating: number}>`
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  border-radius: 16px;
  font-weight: 600;
  font-size: 16px;
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
