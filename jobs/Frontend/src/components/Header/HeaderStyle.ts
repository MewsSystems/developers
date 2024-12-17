import styled from "styled-components";

export const Header = styled.header`
  width: 100%;
  background: transparent;
  backdrop-filter: blur(4px);
  display: flex;
  justify-content: center;
  align-items: center;
  padding: ${({ theme }) => theme.spacing(2)} 0;
`;

export const AppName = styled.h1`
  margin: 0;
  font-size: 1.8rem;
  font-weight: 700;
  color: ${({ theme }) => theme.colors.textPrimary};
`;
