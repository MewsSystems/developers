import { ExclamationCircle } from '@styled-icons/fa-solid';
import EmptyState, { EmptyStateProps } from './EmptyState';

interface ErrorProps extends Omit<EmptyStateProps, 'icon'> {}

const ErrorState = ({ title, children }: ErrorProps) => {
  return (
    <EmptyState title={title} icon={<ExclamationCircle size="5rem" />}>
      {children}
    </EmptyState>
  );
};

export default ErrorState;
