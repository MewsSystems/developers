import { Film } from '@styled-icons/fa-solid';
import { EmptyStateContainer, EmptyStateBody, EmptyStateTitle } from './styled';

export interface EmptyStateProps {
  icon?: React.ReactNode;
  title?: string;
  children?: React.ReactNode;
}

const EmptyState = ({ icon, title, children }: EmptyStateProps) => {
  return (
    <EmptyStateContainer>
      {icon || <Film size="5rem" />}
      {title && <EmptyStateTitle>{title}</EmptyStateTitle>}
      {children && <EmptyStateBody>{children}</EmptyStateBody>}
    </EmptyStateContainer>
  );
};

export default EmptyState;
