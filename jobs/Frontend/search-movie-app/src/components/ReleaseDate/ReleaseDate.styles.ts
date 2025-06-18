import styled from 'styled-components';

export const StyledReleaseDate = styled.div`
  display: flex;
  background: ${({ theme }) => theme.colors.primary};
  padding: 2px 6px;
  border-radius: 6px;
  backdrop-filter: blur(10px);
  color: ${({ theme }) => theme.colors.onPrimary};
  align-items: center;
  font-weight: 500;
  margin: 10px;
`;
