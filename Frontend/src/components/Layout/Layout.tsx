import React from 'react';
import { PageHeader, PageContainer, PageFooter, PageMain } from './styled';

interface LayoutProps {
  header: React.ReactNode;
  children: React.ReactNode;
  footer?: React.ReactNode;
  isFullWidth?: boolean;
}

const Layout = ({
  header,
  footer,
  children,
  isFullWidth = true,
}: LayoutProps) => {
  return (
    <PageContainer isFullWidth={isFullWidth}>
      <PageHeader>{header}</PageHeader>
      <PageMain>{children}</PageMain>
      {footer && <PageFooter>{footer}</PageFooter>}
    </PageContainer>
  );
};

export default Layout;
