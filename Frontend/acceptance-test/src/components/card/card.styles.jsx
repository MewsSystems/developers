import styled from "styled-components";
import "../../styles/keyFrames.scss";
import "../../styles/variables.scss";

export const Cards = styled.div`
  height: 35rem;
  padding: 1.5rem;
  border-radius: 1rem;
  margin: 15rem 0;
  transition: all 0.7s ease;
  margin: 5rem 1rem 2rem 1rem;
  overflow: hidden;
  background-image: url(${props => props.imageBg});
  background-repeat: no-repeat;
  background-size: cover;
  -webkit-filter: drop-shadow(0 1.5rem 4rem rgba(0, 0, 0, 0.15));

  display: flex;
  justify-content: center;
  align-items: flex-end;

  &:hover {
    animation: animateScale 1s infinite;
  }
`;

export const MovieTitle = styled.h5`
  color: white;
  opacity: 1;
  font-size: 1.7rem;
`;
