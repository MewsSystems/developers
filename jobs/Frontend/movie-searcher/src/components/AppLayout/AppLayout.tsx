import { Col, Layout, Row } from "antd";
import { Header } from "antd/es/layout/layout";
import { ReactNode } from "react";
import { Link } from "react-router-dom";
import { Heading, FooterStyled, LayoutStyled, ContentStyled } from "./AppLayout.styled";

type AppLayoutProps = {
  children: ReactNode;
};

const AppLayout = ({ children }: AppLayoutProps) => (
  <LayoutStyled>
    <Header>
      <Link to="/">
        <Heading>Movie Lookup</Heading>
      </Link>
    </Header>
    <Layout>
      <ContentStyled>
        <Row>
          <Col span={18} offset={3}>
            {children}
          </Col>
        </Row>
      </ContentStyled>
    </Layout>
    <FooterStyled>Have a good day! =)</FooterStyled>
  </LayoutStyled>
);

export { AppLayout };
