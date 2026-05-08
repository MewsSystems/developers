import styled from 'styled-components';

const StyledVote = styled.div`
  display: flex;
  background: ${({ theme }) => theme.colors.primary};
  padding: 2px 6px;
  border-radius: 6px;
  backdrop-filter: blur(10px);
  align-items: center;
  color: ${({ theme }) => theme.colors.onPrimary};
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  font-weight: 500;
`;

const StyledVoteNumber = styled.a`
  color: ${({ theme }) => theme.colors.onPrimary};

  margin-right: 5px;
`;

export { StyledVote, StyledVoteNumber };
