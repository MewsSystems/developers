import styled from "styled-components";

export const MovieCardContainer = styled.div`
  background: ${({ theme }) => theme.colors.cardBackground};
  border-radius: 8px;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  display: flex;
  flex-direction: column;
  cursor: pointer;
  &:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
  }
`;

export const Poster = styled.img`
  width: 100%;
  height: auto;
  display: block;
`;

export const MovieInfo = styled.div`
  padding: ${({ theme }) => theme.spacing(2)};
`;

export const Title = styled.h2`
  margin: 0 0 ${({ theme }) => theme.spacing(1)} 0;
  font-size: 1rem;
  font-weight: 600;
  color: ${({ theme }) => theme.colors.textPrimary};
`;

export const SubInfo = styled.div`
  font-size: 0.85rem;
  color: ${({ theme }) => theme.colors.textSecondary};
  display: flex;
  justify-content: space-between;
  align-items: center;
`;
