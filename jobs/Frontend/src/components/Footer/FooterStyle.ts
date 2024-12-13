import styled from "styled-components";

export const Footer = styled.footer`
  margin-top: auto;
  padding: ${({ theme }) => theme.spacing(2)};
  text-align: center;
  background: transparent;
  color: ${({ theme }) => theme.colors.textSecondary};
  font-size: 0.9rem;
`;
