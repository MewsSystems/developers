import { Outlet } from "react-router";
import { Header } from "../components/organisms/layout/header";
import styled from "styled-components";

export const Layout: React.FC = () => {
  return (
    <VisualAppContainer>
      <Header />
      <MainContainer>
        <Outlet />
      </MainContainer>
    </VisualAppContainer>
  );
};

const VisualAppContainer = styled.div`
  display: flex;
  flex-direction: column;
  min-height: 100vh;
`;

const MainContainer = styled.div`
  background-color: black;
  flex-grow: 1;
`;
