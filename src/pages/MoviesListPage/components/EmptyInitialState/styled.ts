import styled from 'styled-components';

export const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: calc(100vh - 200px); /* Account for header and some padding */
  text-align: center;
  color: #6b7280;
`;

export const Icon = styled.svg`
  margin-bottom: 24px;
  color: #9ca3af;
`;

export const Content = styled.div`
  max-width: 400px;
`;

export const Title = styled.h2`
  font-size: 20px;
  font-weight: 600;
  margin-bottom: 12px;
  color: #374151;
`;

export const Message = styled.p`
  font-size: 16px;
  line-height: 1.5;
`;
