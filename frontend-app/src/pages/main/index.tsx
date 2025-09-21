import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { Button, Flex, Text } from "@chakra-ui/react";
import { Link } from "@tanstack/react-router";
export function MainPageRouteComponent() {
  const auth = useAuth();

  return !auth.isAuthenticated ? (
    <Flex
      direction="column"
      justifyContent="center"
      justifyItems="items"
      alignContent="center"
      alignItems="center"
    >
      <Text>Please login</Text>
      <Button
        width="50%"
        onClick={() => {
          auth.login();
        }}
      >
        Login
      </Button>
    </Flex>
  ) : (
    <Text>You are logged</Text>
  );
}
