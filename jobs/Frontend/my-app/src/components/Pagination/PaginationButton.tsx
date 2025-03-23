import styled from 'styled-components';

const StyledButton = styled.button`
  min-width: 100px;
  border: none;
  background-color: #f9f9f9;
  transition: all 0.3s ease-out;
  &:hover,
  &:focus {
    background-color: #e0e0e0;
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
