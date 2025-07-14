'use client';

import React, { ReactNode, Ref, useState, cloneElement, isValidElement, Children } from 'react';
import { AccessibleResultsSummary, AccessibleResultsSummaryProps } from './index';

interface ResultsSummaryContainerProps extends React.HTMLAttributes<HTMLDivElement> {
  children: ReactNode;
  ref?: Ref<HTMLDivElement>;
}

export function ResultsSummary({
  children,
  className = '',
  ref,
  ...rest
}: ResultsSummaryContainerProps) {
  const [isFocused, setIsFocused] = useState(false);

  const enhancedChildren = Children.map(children, (child) => {
    if (isValidElement(child) && child.type === AccessibleResultsSummary) {
      return cloneElement(child as React.ReactElement<AccessibleResultsSummaryProps>, {
        addSearchGuidance: isFocused,
      });
    }
    return child;
  });

  return (
    <div
      data-testid="results-summary-container"
      className={`min-h-[24px] mt-1 ${className}`}
      ref={ref}
      tabIndex={-1}
      onFocus={() => setIsFocused(true)}
      onBlur={() => setIsFocused(false)}
      {...rest}
    >
      {enhancedChildren}
    </div>
  );
}
