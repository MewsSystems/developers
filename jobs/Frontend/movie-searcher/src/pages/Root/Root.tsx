import { Outlet } from "react-router-dom";
import { AppLayout } from "../../components/AppLayout";

const RootPage = () => (
  <AppLayout>
    <Outlet />
  </AppLayout>
);
export { RootPage };
