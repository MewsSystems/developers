import styled from "styled-components";

export const MovieCardImage = styled.img`
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: all 0.3s;
`;

export const MovieCardInfoContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: flex-start;
  gap: 0.2rem;
`;

export const MovieMetaContainer = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  gap: 0.2rem;
`;

export const MovieBadgeInfo = styled.div`
  padding: 0.1rem;
  border: 1px solid #333;
`;

export const MoveCardInfo = styled.p`
  font-size: 0.6rem;
  color: #333;
  font-weight: 700;
`;

export const MovieCardHoveredContent = styled.div`
  position: absolute;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 0.6rem;
  color: #333;
  width: 100%;
  min-height: 20%;
  height: fit-content;
  bottom: 0;
  left: 0;
  background-color: #fff;
`;
