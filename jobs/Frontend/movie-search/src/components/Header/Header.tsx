import { StyledHeader } from "./Header.internal";

interface HeaderProps {
  children: React.ReactNode;
}

export const Header = (props: HeaderProps) => {
  return <StyledHeader>{props.children}</StyledHeader>;
};
