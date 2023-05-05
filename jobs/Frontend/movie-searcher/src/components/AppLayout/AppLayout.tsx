import { Col, Layout, Row } from "antd";
import { Header } from "antd/es/layout/layout";
import { ReactNode } from "react";
import { StyledH1, StyledFooter, StyledLayout, StyledContent } from "./AppLayout.styled";

type AppLayoutProps = {
  children: ReactNode;
};

const AppLayout = ({ children }: AppLayoutProps) => (
  <StyledLayout>
    <Header>
      <StyledH1>Movie Lookup</StyledH1>
    </Header>
    <Layout>
      <StyledContent>
        <Row>
          <Col span={18} offset={3}>
            {children}
          </Col>
        </Row>
      </StyledContent>
    </Layout>
    <StyledFooter>Have a good day! =)</StyledFooter>
  </StyledLayout>
);

export { AppLayout };
