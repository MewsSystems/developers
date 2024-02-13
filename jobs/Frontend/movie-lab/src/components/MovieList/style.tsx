import styled, { keyframes } from "styled-components";

export const Grid = styled.div`
  display: grid;
  margin: 20px;
  gap: 20px;
  justify-content: center;

  @media screen and (min-width: 768px) {
    grid-template-columns: repeat(5, 1fr); 
  }

  @media screen and (max-width: 767px) {
    grid-template-columns: repeat(2, 1fr);
  }
`;

export const Card = styled.div`
  position: relative;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.2);
  cursor: pointer;
  transition: transform 0.3s ease;

  &:hover {
    transform: scale(1.05);

    &::before {
      content: '';
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background: rgba(0, 0, 0, 0.5);
      z-index: 1;
    }

    h3,
    p {
      visibility: visible;
      opacity: 1;
    }
  }
`;

export const MovieImage = styled.img`
  object-fit: cover;
  width: 100%;
  height: 100%;
`;

export const MovieDetails = styled.div`
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  text-align: center;
  color: #fff;
  z-index: 2;
  visibility: hidden;
  opacity: 0;
  transition: visibility 0s, opacity 0.3s ease;
`;

export const fadeIn = keyframes`
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
`;

export const ModalWrapper = styled.div`
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background: rgba(0, 0, 0, 0.7);
  animation: ${fadeIn} 0.3s ease;
`;

export const ModalContent = styled.div`
  background-color: rgba(24, 24, 24, 0.9);
  padding: 40px;
  border-radius: 20px;
  box-shadow: 0px 0 20px 0px rgba(0, 0, 0, 0.2);
  display: flex;
  max-width: 90%;
  max-height: 90%;
  overflow: auto;
  position: relative;

  @media screen and (max-width: 425px) {
    flex-direction: column;
  }
`;

export const LeftColumn = styled.div`
  flex: 1;
  padding-right: 20px;
`;

export const RightColumn = styled.div`
  flex: 2;
  color: white;
  max-width: 300px;
`;

export const CloseButton = styled.span`
  color: lightcyan;
  position: absolute;
  top: 20px;
  right: 20px;
  cursor: pointer;
  font-size: 24px;

  &:hover {
    color: white;
  }
`;

export const Image = styled.img`
  width: auto;
  max-width: 100%;
  height: 100%;
`;
