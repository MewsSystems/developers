interface PaginationButtonProps {
  direction: string;
  onClick: () => void;
}

export const PaginationButton: React.FC<PaginationButtonProps> = ({
  direction,
  onClick,
}) => {
  return <button onClick={onClick}>{direction}</button>;
};
