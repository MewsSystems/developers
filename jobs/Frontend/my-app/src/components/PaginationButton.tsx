interface PaginationButtonProps {
  direction: string;
}

export const PaginationButton: React.FC<PaginationButtonProps> = ({
  direction,
}) => {
  return <button>{direction}</button>;
};
