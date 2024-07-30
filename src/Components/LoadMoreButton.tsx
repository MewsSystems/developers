import React from 'react';

interface LoadMoreButtonProps {
  onLoadMore: () => void;
  hasMore: boolean;
}

const LoadMoreButton: React.FC<LoadMoreButtonProps> = ({ onLoadMore, hasMore }) => {
  if (!hasMore) return null;

  return (
    <button onClick={onLoadMore} className="next">
      Next 20 movies...
    </button>
  );
};

export default LoadMoreButton;
