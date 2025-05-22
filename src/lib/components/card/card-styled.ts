import { styled } from "styled-components";

interface StyledCardProps {
  variant?: 'default' | 'movie' | 'profile';
}

export const StyledCard = styled.div<StyledCardProps>`
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.2s ease-in-out;
  display: flex;
  flex-direction: column;
  height: 100%;
  
  &:hover {
    transform: translateY(-4px);
  }
`;

export const CardHeader = styled.div`
  padding: 1rem;
  border-bottom: 1px solid #eee;
`;

export const CardBody = styled.div`
  padding: 1rem;
  flex: 1;
  display: flex;
  flex-direction: column;
`;

export const CardFooter = styled.div`
  padding: 1rem;
  border-top: 1px solid #eee;
  background: #f8f9fa;
  margin-top: auto;
`;

export const CardImage = styled.img`
  width: 100%;
  height: auto;
  object-fit: cover;
  object-position: center;
  display: block;
`;

export const CardImageContainer = styled.div`
  aspect-ratio: 2/3;
  overflow: hidden;
`;

export const CardTitle = styled.h3`
  margin: 0;
  font-size: 1.25rem;
  color: #333;
  margin-bottom: 0.5rem;
`;

export const CardDescription = styled.p`
  margin: 0;
  color: #666;
  font-size: 0.875rem;
  line-height: 1.4;
  display: -webkit-box;
  -webkit-box-orient: vertical;
`;