import styled from 'styled-components';

export const ClearButton = styled.button`
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  padding: 6px 8px;
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

export const SearchContainer = styled.div`
  position: relative;
  max-width: 800px;
  margin: 0 auto;
  width: 100%;
`;

export const SearchInput = styled.input`
  width: 100%;
  padding: 10px 80px 10px 16px;
  font-size: 15px;
  color: #5a5d63;
  background-color: #f3f4f6;
  border: none;
  border-radius: 6px;
  outline: none;
  transition: all 0.2s ease;

  &:focus {
    background-color: #e5e7eb;
  }

  &::placeholder {
    color: #9ca3af;
  }
`;

export const SearchWarning = styled.div<{isError: boolean}>`
  color: ${({isError}) => (isError ? '#dc2626' : '#d97706')};
  font-size: 14px;
  margin-top: 4px;
  position: absolute;
`;
