import React, { ReactNode } from 'react';
import { Button as BaseButton } from '@mui/material';

type ButtonSize = 'small' | 'large';

interface Props {
  onClick?: () => void;
  children: ReactNode;
  size?: ButtonSize;
}

export default function Button({ onClick, children, size = 'large' }: Props) {
  return (
    <BaseButton
      color="primary"
      variant="contained"
      size={size}
      onClick={onClick}
    >
      {children}
    </BaseButton>
  );
}
