import styled from "styled-components";

export const StyledGrid = styled.div`
  width: 100%;
  margin: 0 auto;
  padding: 1.8rem 0;
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(10rem, 1fr));
  align-items: stretch;
  justify-content: flex-start;
  gap: 1.2rem;
`;

interface StyledGridCardProps {
  $isHovered: boolean;
}

export const StyledGridCard = styled.div<StyledGridCardProps>`
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  border: none;
  overflow: hidden;

  transition: all 0.3s ease-out;
  cursor: pointer;

  transform: ${(props) => (props.$isHovered ? "scale(1.35)" : "scale(1)")};
  z-index: ${(props) => (props.$isHovered ? "50" : "1")};
  box-shadow: ${(props) =>
    props.$isHovered ? "0 25px 50px -12px rgba(0, 0, 0, 0.25)" : "none"};
`;
