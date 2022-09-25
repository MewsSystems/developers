import styled from "styled-components";

export const List = styled.ul`
  display: grid;
  gap: 3rem 1.5rem;
  padding: 0;
  padding: 3rem 0 8rem;
  list-style: none;
  grid-template-columns: repeat(auto-fit, minmax(20rem, 1fr));
  width: 100%;
`;
