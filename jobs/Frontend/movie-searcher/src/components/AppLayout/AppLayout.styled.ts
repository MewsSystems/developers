import styled from "styled-components";
import { Footer, Content } from "antd/es/layout/layout";
import { Layout } from "antd";

const StyledH1 = styled.h1`
  color: ${(p) => p.theme.colors.primary};
  font-size: ${(p) => p.theme.fontSize.h1};
  font-weight: ${(p) => p.theme.fontWeight.bold};
  text-align: center;
`;

const StyledFooter = styled(Footer)`
  margin-top: auto;
  background-color: ${(p) => p.theme.colors.primary};
`;

const StyledLayout = styled(Layout)`
  min-height: 100vh;
`;

const StyledContent = styled(Content)`
  padding: 24px;
`;

export { StyledH1, StyledFooter, StyledLayout, StyledContent };
