import styled from 'styled-components';
import Box from '@/components/Box';

export const MovieHeading = styled(Box).attrs((attrs) => ({...attrs, as: 'h1'}))`
  font-size: 4em;
  position: absolute;
  bottom: 72px;
  left: 24px;
  text-shadow: -3px 1px 19px #5d5757;
  color: white;
`;
