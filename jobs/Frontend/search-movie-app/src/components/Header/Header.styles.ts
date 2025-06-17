import styled from 'styled-components';

export const StyledHeaderWrapper = styled.div`
  display: flex;
  width: 100%;
  align-items: center;
  justify-content: center;
  position: relative;
`;
export const StyledHeaderText = styled.h1`
  display: flex;
  margin: 0;
`;
export const StyledToggle = styled.button`
  display: flex;
  background-color: transparent;
  color: ${({ theme }) => theme.colors.primary};
  font-size: 30px;
  border: none;
  cursor: pointer;
  transition: transform 200ms ease-in-out;
  &:hover {
    color: ${({ theme }) => theme.colors.hoverPrimary};
    transform: scale(1.1);
  }
  position: absolute;
  right: 0;
`;
