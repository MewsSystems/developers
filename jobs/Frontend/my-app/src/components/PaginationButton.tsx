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
    <button onClick={onClick} disabled={disabled}>
      {direction}
    </button>
  );
};
