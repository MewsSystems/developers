import { Outlet } from "react-router";

export default function RootPage() {
  return (
    <div className="flex flex-col max-h-screen gap-3">
      <Outlet />
    </div>
  );
}
