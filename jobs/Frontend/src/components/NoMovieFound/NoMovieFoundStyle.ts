import styled from "styled-components";

export const NoSuchMovieContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  height: 100%;
  text-align: center;
  color: ${({ theme }) => theme.colors.textPrimary};
  padding: ${({ theme }) => theme.spacing(4)};
  gap: ${({ theme }) => theme.spacing(2)};
`;

export const NoSuchMovieIcon = styled.div`
  font-size: 3rem;
`;

export const NoSuchMovieMessage = styled.div`
  font-size: 1.2rem;
  font-weight: 500;
`;
