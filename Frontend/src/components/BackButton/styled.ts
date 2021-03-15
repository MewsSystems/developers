import styled from 'styled-components';
import BaseButton from '../common/Button';

export const Button = styled(BaseButton)`
  white-space: nowrap;
  border: none;
  font-size: inherit;
  padding: 0 ${({ theme }) => theme.space[3]};

  > svg {
    height: 1.6rem;
    padding-bottom: ${({ theme }) => theme.space[2]};
  }
`;
