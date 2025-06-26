import { StyledWrapper } from "./LayoutWrapper.internal";

interface LayoutWrapperProps {
  children: React.ReactNode;
}

export const LayoutWrapper = (props: LayoutWrapperProps) => {
  return <StyledWrapper>{props.children}</StyledWrapper>;
};
