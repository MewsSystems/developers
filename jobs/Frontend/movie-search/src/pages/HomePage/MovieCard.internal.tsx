import styled from "styled-components";

export const MovieCardImage = styled.img`
  width: 100%;
  height: auto;
  object-fit: contain;
`;

export const MovieMetaContainer = styled.div`
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  gap: 0.2rem;
`;

export const MoveCardInfo = styled.p`
  font-size: 0.6rem;
  color: #000;
  font-weight: 700;
`;

export const MovieBadgeInfo = styled.div`
  padding: 0.1rem;
  border: 1px solid #333;
`;

export const MovieCardHoveredContent = styled.div`
  position: absolute;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  padding: 0.6rem;
  color: #333;
  width: 100%;
  height: 20%;
  bottom: 0;
  left: 0;
  background-color: #fff;
`;
