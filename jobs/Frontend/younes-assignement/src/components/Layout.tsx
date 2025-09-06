import { PageContainer } from "../styles/styles";

type LayoutProps = {
  children: React.ReactNode;
};

const Layout = ({ children }: LayoutProps) => {
  return <PageContainer>{children}</PageContainer>;
};

export default Layout;
