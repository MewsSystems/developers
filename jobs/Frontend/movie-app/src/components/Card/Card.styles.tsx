import styled from "styled-components";

export const CardWrapper = styled.div`
  display: block;
  width: 220px;
  height: 330px;
  background: #efefef;
  border-radius: 1rem;
  animation: loading 1s linear infinite alternate;
  @keyframes loading {
    0% {
      background-color: hsl(200, 20%, 80%);
    }
    100% {
      background-color: hsl(200, 20%, 95%);
    }
  }
`;

export const CardImage = styled.img`
  border-radius: 1rem;
  transition: all 0.3s ease-in-out;
  &:hover {
    transform: scale(1.05);
    opacity: 0.8;
  }
  width: 220px;
  height: 330px;
`;
