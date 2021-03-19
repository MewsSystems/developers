import EmptyState, { EmptyStateProps } from './EmptyState';

export interface LoadingStateProps extends Omit<EmptyStateProps, 'children'> {}

function LoadingState({ icon, title = 'Loading...' }: LoadingStateProps) {
  return <EmptyState title={title} icon={icon} />;
}

export default LoadingState;
