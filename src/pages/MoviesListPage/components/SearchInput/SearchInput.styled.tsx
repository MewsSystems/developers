import styled from 'styled-components';

export const SearchContainer = styled.div`
  position: relative;
  max-width: 800px;
  margin: 0 auto;
  width: 100%;
`;

export const SearchInput = styled.input`
  width: 100%;
  padding: 12px 80px 12px 16px;
  font-size: 16px;
  color: #878787;
  background-color: transparent;
  border: none;
  border-bottom: 2px solid #e5e7eb;
  outline: none;
  transition: all 0.2s ease;

  &:focus {
    border-color: #6366f1;
  }

  &::placeholder {
    color: #9ca3af;
  }
`;

export const ClearButton = styled.button`
  position: absolute;
  right: 0;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  padding: 6px 0 6px 8px;
  cursor: pointer;
  color: #9ca3af;
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 14px;
  line-height: 1;
  transition: color 0.2s ease;

  &:hover {
    color: #6b7280;
  }

  svg {
    width: 14px;
    height: 14px;
    display: block;
  }
`;

export const SearchWarning = styled.div<{isError: boolean}>`
  color: ${({isError}) => (isError ? '#dc2626' : '#d97706')};
  font-size: 14px;
  margin-top: 8px;
  position: absolute;
`;
