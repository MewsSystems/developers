import React from 'react';

interface Props {
  children: React.ReactNode;
}

const Tag = ({ children }: Props) => {
  return (
    <div className='rounded-lg bg-accent text-accent-foreground px-2 py-1 max-w-min whitespace-nowrap'>{children}</div>
  );
};

export default Tag;
