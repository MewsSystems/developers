import styled from 'styled-components';

export const PaginationWrapper = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 8px;
  padding: 16px 0 24px;
`;

export const NavigationButton = styled.button<{$isCompact?: boolean}>`
  padding: ${({$isCompact}) => ($isCompact ? '8px' : '8px 12px')};
  font-size: 14px;
  font-weight: 500;
  color: ${({disabled}) => (disabled ? '#9ca3af' : '#374151')};
  background-color: ${({disabled}) => (disabled ? '#f3f4f6' : '#ffffff')};
  border: 1px solid ${({disabled}) => (disabled ? '#e5e7eb' : '#d1d5db')};
  border-radius: 6px;
  cursor: ${({disabled}) => (disabled ? 'not-allowed' : 'pointer')};
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  min-width: ${({$isCompact}) => ($isCompact ? '36px' : '80px')};

  &:not(:disabled):hover {
    background-color: #f9fafb;
    border-color: #9ca3af;
  }
`;

export const PageInfo = styled.span`
  color: #4b5563;
  font-size: 14px;
  margin: 0 4px;
`;
