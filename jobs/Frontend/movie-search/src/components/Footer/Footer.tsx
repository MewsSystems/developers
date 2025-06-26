import { StyledFooter } from "./Footer.internal";

interface FooterProps {
  children: React.ReactNode;
}

export const Footer = (props: FooterProps) => {
  return <StyledFooter>{props.children}</StyledFooter>;
};
