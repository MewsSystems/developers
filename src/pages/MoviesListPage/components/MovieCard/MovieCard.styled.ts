import {Link} from 'react-router-dom';
import styled from 'styled-components';

export const StyledLink = styled(Link)`
  text-decoration: none;
  color: inherit;
  height: 100%;
  display: block;
`;

export const Card = styled.div`
  display: flex;
  flex-direction: column;
  height: 100%;
  background: white;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 1px 3px 0 rgb(0 0 0 / 0.1), 0 1px 2px -1px rgb(0 0 0 / 0.1);
  transition: transform 0.2s ease-in-out;

  &:hover {
    transform: translateY(-4px);
  }
`;

export const PosterContainer = styled.div`
  position: relative;
  width: 100%;
  height: 150px;
  background: #f3f4f6;
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
  align-items: center;
  justify-content: center;
`;

export const Content = styled.div`
  padding: 16px;
  flex: 1;
  display: flex;
  flex-direction: column;
`;

export const Title = styled.h2`
  margin: 0 0 8px;
  font-size: 16px;
  font-weight: 600;
  color: #111827;
  display: -webkit-box;
  -webkit-line-clamp: 1;
  -webkit-box-orient: vertical;
  overflow: hidden;
`;

export const Overview = styled.p`
  margin: 0 0 16px;
  font-size: 14px;
  color: #6b7280;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  line-height: 1.5;
  flex: 1;
`;

export const MetaInfo = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: #6b7280;
  font-size: 14px;
  margin-top: auto;
`;

export const Rating = styled.span<{$rating: number}>`
  padding: 2px 8px;
  border-radius: 4px;
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
