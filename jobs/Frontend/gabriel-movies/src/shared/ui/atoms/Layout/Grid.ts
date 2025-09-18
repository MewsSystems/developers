import styled from "styled-components";

export const Grid = styled.div`
  display: grid;
  gap: 16px;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  
  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;
