import React from 'react';

interface Props {
  children: React.ReactNode;
}

const Tag = ({ children }: Props) => {
  return (
    <div className='rounded-lg bg-slate-600/80 px-2 py-1 max-w-min'>{children}</div>
  );
};

export default Tag;
