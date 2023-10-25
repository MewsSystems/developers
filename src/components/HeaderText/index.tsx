import React, { ReactNode } from 'react';
import { Text } from './styles';

export const HeaderText = ({ children }: { children: ReactNode }) => {
  return <Text>{children}</Text>;
};
