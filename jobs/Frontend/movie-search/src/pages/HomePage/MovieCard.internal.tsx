import styled from "styled-components";

export const MovieCardImage = styled.img`
  width: 100%;
  height: auto;
  object-fit: contain;
`;

export const MovieMetaContainer = styled.div`
  display: flex;
  flex-direction: column;
  gap: 0.1rem;
`;

export const MovieCardTitle = styled.p`
  font-size: 0.6rem;
  color: #000;
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
  height: 20%;
  bottom: 0;
  left: 0;
  background-color: #fff;
`;
