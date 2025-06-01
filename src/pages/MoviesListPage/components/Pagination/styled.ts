import styled, {css} from 'styled-components';

type NavigationButtonProps = {
  $active?: boolean;
};

export const PaginationWrapper = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  margin: 24px 0;
`;

export const NavigationButton = styled.button<NavigationButtonProps>`
  padding: 8px 12px;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  background: white;
  color: #4a5568;
  cursor: pointer;
  transition: all 0.2s;
  min-width: 40px;
  display: flex;
  align-items: center;
  justify-content: center;

  &:hover:not(:disabled) {
    background: #e2e8f0;
  }

  &:disabled {
    cursor: not-allowed;
    opacity: 0.5;
  }

  ${(props) =>
    props.$active &&
    css`
      background: #0396a3;
      color: white;
      border-color: #0396a3;

      &:hover {
        background: #475569;
      }
    `}
`;
