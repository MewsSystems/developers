import styled from "styled-components"

export const StyledCard = styled.div`
  width: 300px;
  margin: 10px;
  background-color: ${({ theme }) => theme.colors.secondary};
  box-shadow: 0 4px 5px rgba(0, 0, 0, 0.2);
  position: relative;
  overflow: hidden;
  border-radius: 3px;

  &:hover {
    box-shadow: 0 4px 5px rgba(255, 255, 255, 0.1);
  }
`
