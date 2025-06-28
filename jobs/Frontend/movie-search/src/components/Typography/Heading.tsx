import { StyledHeading } from "./Heading.internal";

interface HeadingProps {
  children: React.ReactNode;
}

export const Heading = (props: HeadingProps) => {
  return <StyledHeading>{props.children}</StyledHeading>;
};
