import styled from 'styled-components';
import {Link} from 'react-router-dom';

export const StyledLink = styled(Link)`
  height: 100%;
`;

export const Card = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition:
    transform 0.2s ease-in-out,
    box-shadow 0.2s ease-in-out;

  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  }
`;

export const PosterContainer = styled.div`
  width: 100%;
  height: 150px;
  background-color: #e5e7eb;
  position: relative;
`;

export const PosterImage = styled.img`
  width: 100%;
  height: 100%;
  object-fit: cover;
`;

export const PosterPlaceholder = styled.div`
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background: linear-gradient(
    45deg,
    #e5e7eb 25%,
    #d1d5db 25%,
    #d1d5db 50%,
    #e5e7eb 50%,
    #e5e7eb 75%,
    #d1d5db 75%
  );
  background-size: 20px 20px;
  color: #6b7280;
`;

export const PlaceholderIcon = styled.div`
  width: 40px;
  height: 40px;
  margin-bottom: 8px;
  border: 2px solid currentColor;
  border-radius: 4px;
  display: flex;
  align-items: center;
  justify-content: center;

  &::before {
    content: '🎬';
    font-size: 20px;
  }
`;

export const Content = styled.div`
  padding: 12px;
  flex: 1;
  display: flex;
  flex-direction: column;
`;

export const Title = styled.h3`
  font-size: 16px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 4px;
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
`;

export const Overview = styled.p`
  font-size: 14px;
  color: #6b7280;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  margin-bottom: 8px;
  flex: 1;
`;

export const MetaInfo = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 12px;
  color: #4b5563;
  margin-top: auto;
`;

export const Rating = styled.span<{$rating: number}>`
  display: flex;
  align-items: center;
  gap: 4px;
  color: ${({$rating}) => ($rating >= 7 ? '#059669' : $rating >= 5 ? '#d97706' : '#dc2626')};
  font-weight: 500;

  &::after {
    content: '★';
  }
`;

export const RatingBadge = styled.div<{$rating: number}>`
  position: absolute;
  top: 8px;
  right: 8px;
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 14px;
  font-weight: 500;
  color: white;
  background-color: ${({$rating}) =>
    $rating >= 7 ? '#059669' : $rating >= 5 ? '#d97706' : '#dc2626'};
`;
