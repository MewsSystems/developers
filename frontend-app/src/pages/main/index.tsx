import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { Text } from "@chakra-ui/react";
import { Login } from "../login";
export function MainPageRouteComponent() {
  const auth = useAuth();

  return !auth.isAuthenticated ? <Login /> : <Text>You are logged</Text>;
}
