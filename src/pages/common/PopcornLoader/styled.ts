import styled from 'styled-components';

import {bounceAndRotate} from './animations';

export const LoadingContainer = styled.div`
  width: 100%;
  min-height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;

  img {
    animation: ${bounceAndRotate} 0.2s ease-in-out infinite;
    transform-origin: center bottom;
  }
`;
