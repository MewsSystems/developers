import { ComponentPropsWithoutRef } from 'react';

interface CardProps extends ComponentPropsWithoutRef<'div'> {
  className?: string;
}

export function Card({ children, className = '', ...rest }: CardProps) {
  return (
    <div
      className={`flex grow bg-white rounded-xl border border-stone-200 shadow shadow-stone-200 ${className}`}
      {...rest}
    >
      {children}
    </div>
  );
}
