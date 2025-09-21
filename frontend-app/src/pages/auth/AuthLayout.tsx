import useQueryAccount from "@/entities/account/hooks/useQueryAccount";
import { Outlet } from "@tanstack/react-router";

export function AuthLayout() {
  const { data: account, isLoading } = useQueryAccount();

  if (isLoading) {
    return <div>Loading...</div>;
  }
  if (!account) {
    return <></>;
  }

  return <Outlet />;
}
