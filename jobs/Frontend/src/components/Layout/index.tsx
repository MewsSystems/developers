import { Outlet } from "react-router-dom";
import { Container } from "./components/Container";

export function Layout() {
  return (
    <>
      <Container>
        <Outlet />
      </Container>
    </>
  );
}
