import styled from 'styled-components';
import {spinEffect} from './animations.ts';

export const LoadingOverlay = styled.div`
  position: fixed;
  inset: 0;
  background-color: white;
  display: flex;
  justify-content: center;
  align-items: center;
`;

export const Spinner = styled.div`
  width: 48px;
  height: 48px;
  border: 4px solid #e5e7eb;
  border-top-color: #3b82f6;
  border-radius: 50%;
  animation: ${spinEffect} 1s linear infinite;
`;
