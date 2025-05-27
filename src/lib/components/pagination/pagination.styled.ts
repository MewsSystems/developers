import styled from 'styled-components';

export const PaginationContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.5rem;
  margin: 2rem 0;
`;

export const PageButton = styled.button<{ isActive?: boolean }>`
  padding: 0.5rem 1rem;
  border: 1px solid ${(props) => (props.isActive ? '#007bff' : '#dee2e6')};
  background: ${(props) => (props.isActive ? '#007bff' : 'white')};
  color: ${(props) => (props.isActive ? 'white' : '#007bff')};
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;

  &:hover {
    background: ${(props) => (props.isActive ? '#007bff' : '#f8f9fa')};
  }

  &:disabled {
    background: #e9ecef;
    color: #6c757d;
    cursor: not-allowed;
    border-color: #dee2e6;
  }
`;

export const PageInfo = styled.span`
  color: #6c757d;
  font-size: 0.875rem;
`;
