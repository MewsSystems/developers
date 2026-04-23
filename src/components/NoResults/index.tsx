import React, { ReactNode } from 'react';
import { NoResultsWrapper } from './styled';

export const NoResults = ({ children }: { children: ReactNode }) => {
  return <NoResultsWrapper>{children}</NoResultsWrapper>;
};
