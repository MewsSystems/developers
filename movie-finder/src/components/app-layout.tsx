import React from "react";
import styled from "styled-components";
import { useSelector } from "react-redux";
import { appSelectors } from "../redux/selectors";
import { Loading } from "./loading";

interface IProps {
  children: React.ReactNode;
}

export const AppLayout: React.FC<IProps> = (props) => {
  const { children } = props;
  const loading = useSelector(appSelectors.getLoading);

  return (
    <Layout>
      <PageContent>{children}</PageContent>
      <Loading loading={loading} />
    </Layout>
  );
};

const PageContent = styled.main`
  grid-area: content;
  position: relative;
  width: 100%;
  height: 100%;
`;
const Layout = styled.div`
  position: relative;
  display: grid;
  grid-template-areas: "content";
  grid-template-rows: 1fr;
  grid-template-columns: 1fr;
  height: 100%;
  width: 100%;
`;
