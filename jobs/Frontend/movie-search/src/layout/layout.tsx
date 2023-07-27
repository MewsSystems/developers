import { Outlet } from "react-router-dom";
import styled from "styled-components";
import HeaderComponent from "../components/header/header";

const HeaderLayout = styled.header`
  display:block
  height: 10vh;
`;

const FooterLayout = styled.header`
  display:block
  height: 10vh;
`;
const ContentLayout = styled.section`
 display:block
  height: 100vh;
`;

const PageLayout = styled.div`
  display: block;
`;

export default function Layout() {
  return (
    <PageLayout>
      {/* A "layout route" is a good place to put markup you want to
            share across all the pages on your site, like navigation. */}
      <HeaderLayout>
        <HeaderComponent></HeaderComponent>
      </HeaderLayout>

      <ContentLayout>
        <Outlet />
      </ContentLayout>

      <FooterLayout>footer</FooterLayout>
    </PageLayout>
  );
}
