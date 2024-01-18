import styled from 'styled-components';
import Flex from '@/components/Flex';

export const MovieCard = styled(Flex)`
  border-radius: 10px;
  overflow: hidden; 
  box-shadow: -5px 4px 8px rgb(14 16 17 / 17%);
  background-color: white;
  text-decoration: none;
  color: unset;
  
  transition: box-shadow 0.15s ease-in-out;
  
  &:hover {
    box-shadow: -5px 4px 16px rgb(14 16 17 / 34%);
  }
`;
