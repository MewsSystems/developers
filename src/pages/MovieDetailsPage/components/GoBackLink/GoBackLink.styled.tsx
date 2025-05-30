import styled from 'styled-components';
import {Link} from 'react-router-dom';

export const BackLink = styled(Link)`
  display: inline-flex;
  align-items: center;
  gap: 8px;
  color: #6b7280;
  text-decoration: none;
  font-size: 16px;
  margin-bottom: 32px;
  transition: color 0.2s ease;

  &:hover {
    color: #4b5563;
  }

  svg {
    width: 20px;
    height: 20px;
  }
`;
