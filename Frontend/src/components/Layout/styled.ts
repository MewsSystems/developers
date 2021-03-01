import styled from 'styled-components';
import Container from '../common/Container';

export const PageContainer = styled(Container)`
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
`;

export const PageHeader = styled.header`
  padding: ${(props) => props.theme.space[5]};
  line-height: ${(props) => props.theme.lineHeights.heading};
  background-color: ${(props) => props.theme.colors.primary};
  color: ${(props) => props.theme.colors.secondaryLight};
`;

export const PageMain = styled.main`
  background-color: ${(props) => props.theme.colors.background};
  flex: auto;
  overflow: auto;
`;

export const PageFooter = styled.footer`
  padding-top: ${(props) => props.theme.space[4]};
`;
