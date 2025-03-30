import styled from 'styled-components';

const StyledButton = styled.button`
  min-width: 100px;
  border: none;
  background-color: var(--c-neutral);
  transition: var(--transition-prim);
  &:disabled {
    cursor: default;
    &:hover,
    &:focus {
      background-color: var(--c-neutral);
    }
  }
  &:hover,
  &:focus {
    background-color: var(--c-neutral-hover);
  }
`;

interface PaginationButtonProps {
  direction: string;
  onClick: () => void;
  disabled?: boolean;
}

export const PaginationButton: React.FC<PaginationButtonProps> = ({
  direction,
  onClick,
  disabled = false,
}) => {
  return (
    <StyledButton onClick={onClick} disabled={disabled}>
      {direction}
    </StyledButton>
  );
};
