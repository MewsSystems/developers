import { ReactNode } from 'react';
import Flex from '../common/Flex';
import { FlexProps } from '../common/Flex/Flex';
import { StyledIcon } from '@styled-icons/styled-icon';

interface FlexIconProps {
  icon: StyledIcon;
  children: ReactNode;
  title?: string;
  flexProps?: FlexProps;
}

const FlexIconText = ({
  icon: Icon,
  title,
  children,
  flexProps = { gap: '0 5px', alignItems: 'center' },
}: FlexIconProps) => (
  <Flex {...flexProps} title={title}>
    <Icon size="2rem" />
    {children}
  </Flex>
);

export default FlexIconText;
