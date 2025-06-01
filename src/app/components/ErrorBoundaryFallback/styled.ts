import styled from 'styled-components';

export const ErrorContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 20px;
  text-align: center;
  background-color: #fff;
`;

export const ErrorTitle = styled.h1`
  color: #e53e3e;
  margin-bottom: 16px;
  font-size: 24px;
`;

export const ErrorMessage = styled.p`
  color: #4a5568;
  margin-bottom: 24px;
  max-width: 600px;
`;

export const Button = styled.button`
  background-color: #e53e3e;
  color: white;
  border: none;
  padding: 12px 24px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.2s;
  margin: 0 8px;

  &:hover {
    background-color: #c53030;
  }
`;

export const ButtonGroup = styled.div`
  display: flex;
  gap: 16px;
`;
