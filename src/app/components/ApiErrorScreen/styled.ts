import styled from 'styled-components';

export const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  padding: 20px;
  text-align: center;
  background-color: #fff;
`;

export const ErrorIcon = styled.div`
  font-size: 48px;
  color: #475569;
  margin-bottom: 24px;
`;

export const Title = styled.h1`
  color: #475569;
  margin-bottom: 16px;
  font-size: 24px;
`;

export const Message = styled.p`
  color: #64748b;
  margin-bottom: 24px;
  max-width: 600px;
  line-height: 1.5;
`;

export const Button = styled.button`
  padding: 12px 24px;
  border: none;
  border-radius: 6px;
  background-color: #475569;
  color: white;
  cursor: pointer;
  font-size: 16px;
  transition: background-color 0.2s;

  &:hover {
    background-color: #334155;
  }
`;
