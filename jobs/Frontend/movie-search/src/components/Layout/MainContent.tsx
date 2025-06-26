import { StyledMainContent } from "./MainContent.internal";

interface MainContentProps {
  children: React.ReactNode;
}

export const MainContent = (props: MainContentProps) => {
  return (
    <main>
      <StyledMainContent>{props.children}</StyledMainContent>
    </main>
  );
};
