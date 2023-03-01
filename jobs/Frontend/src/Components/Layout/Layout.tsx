import React, { ReactNode } from "react";
import { InnerWrapper, OuterWrapper } from "./Layout.styled";

interface LayoutProps {
  children: ReactNode;
}

export const Layout = (props: LayoutProps) => {
  const { children } = props;
  
  return(
    <OuterWrapper>
      <InnerWrapper>
        {children}
      </InnerWrapper>
    </OuterWrapper>
  );
};
