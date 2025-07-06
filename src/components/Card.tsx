import { PropsWithChildren } from 'react';

interface MyComponentProps extends PropsWithChildren {
  className?: string;
}

export function Card({ children, className = '', ...rest }: MyComponentProps) {
  return (
    <div
      className={`flex grow bg-white rounded-xl border border-stone-200 shadow shadow-stone-200 ${className}`}
      {...rest}
    >
      {children}
    </div>
  );
}
