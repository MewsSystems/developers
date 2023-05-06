import styled from "styled-components";
import { Footer, Content } from "antd/es/layout/layout";
import { Layout } from "antd";

const Heading = styled.h1`
  color: ${(p) => p.theme.colors.primary};
  font-size: ${(p) => p.theme.fontSize.h1};
  font-weight: ${(p) => p.theme.fontWeight.black};
  text-align: center;
`;

const FooterStyled = styled(Footer)`
  margin-top: auto;
  background-color: ${(p) => p.theme.colors.primary};
`;

const LayoutStyled = styled(Layout)`
  min-height: 100vh;
`;

const ContentStyled = styled(Content)`
  padding: 24px;
`;

export { Heading, FooterStyled, LayoutStyled, ContentStyled };
