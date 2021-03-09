import React from 'react';
import Flex from '../common/Flex';
import { PageHeader, PageFooter, PageMain } from './styled';

interface LayoutProps {
  header: React.ReactNode;
  children: React.ReactNode;
  footer?: React.ReactNode;
}

function Layout({ header, footer, children }: LayoutProps) {
  return (
    <Flex flexDirection="column" flexWrap="nowrap" full>
      <PageHeader>{header}</PageHeader>
      <PageMain>{children}</PageMain>
      {footer && <PageFooter>{footer}</PageFooter>}
    </Flex>
  );
}

export default Layout;
