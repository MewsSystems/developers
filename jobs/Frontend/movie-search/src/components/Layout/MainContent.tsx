import { StyledMainContent } from "./MainContent.internal";

interface MainContentProps {
  children: React.ReactNode;
}

export const MainContent = (props: MainContentProps) => {
  return <StyledMainContent>{props.children}</StyledMainContent>;
};
