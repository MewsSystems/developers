import styled from 'styled-components';

export const Title = styled.h1`
  > small {
    font-style: italic;
    color: ${({ theme }) => theme.colors.secondaryDark};
  }
`;
