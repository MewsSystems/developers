import { useEffect, useState } from 'react';
import EmptyState, { EmptyStateProps } from './EmptyState';

export interface LoadingStateProps extends Omit<EmptyStateProps, 'children'> {
  delay?: number;
}

function LoadingState({
  icon,
  title = 'Loading...',
  delay = 300,
}: LoadingStateProps) {
  const [showLoading, setShowLoading] = useState(false);

  useEffect(() => {
    const timer = setTimeout(() => setShowLoading(true), delay);
    return () => clearTimeout(timer);
  }, [delay, setShowLoading]);

  return <>{showLoading && <EmptyState title={title} icon={icon} />}</>;
}

export default LoadingState;
