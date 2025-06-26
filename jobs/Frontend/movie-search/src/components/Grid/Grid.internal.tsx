import styled from "styled-components";

export const StyledGrid = styled.div`
  width: 100%;
  margin: 0 auto;
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(10rem, 1fr));
  align-items: center;
  gap: 1.2rem;
`;

export const StyledGridCard = styled.div`
  border: 1px solid red;
  padding: 1rem;
  height: 15rem;
  display: flex;
  align-items: center;
  justify-content: center;
`;
