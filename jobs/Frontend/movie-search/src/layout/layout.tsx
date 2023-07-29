import { FC } from "react";
import { Outlet } from "react-router-dom";
import Styled from "styled-components";
import HeaderComponent from "../components/header/header";

const HeaderLayout = Styled.header`
  max-height: 200px;
  position: sticky;
  top: 0;
  box-shadow: 0px 20px 10px 0px rgba(0, 0, 0, 0.51);
`;

const FooterLayout = Styled.div`
  max-height: 80px;
  background-color:  rgb(0, 150, 12);;
  color:#f3f3f3;

`;
const ContentLayout = Styled.section``;

const PageLayout = Styled.div`
  display: flex;
  flex-flow: column;
`;

const Layout: FC<{}> = () => {
  return (
    <PageLayout>
      {/* A "layout route" is a good place to put markup you want to
            share across all the pages on your site, like navigation. */}
      <HeaderLayout className='header-layout'>
        <HeaderComponent></HeaderComponent>
      </HeaderLayout>

      <ContentLayout className='content-layout'>
        <Outlet />
      </ContentLayout>

      <FooterLayout>Justin Rios - Task for Mews</FooterLayout>
    </PageLayout>
  );
};

export default Layout;
