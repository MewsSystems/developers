import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { Box, Button, Flex, Text } from "@chakra-ui/react";

export function Login() {
  const auth = useAuth();
  return (
    <Box height={"100%"}>
      <Flex
        height="50%"
        direction="column"
        justifyContent="center"
        justifyItems="items"
        alignContent="center"
        alignItems="center"
      >
        <Text>Please login</Text>
        <Button
          width="25%"
          onClick={() => {
            auth.login();
          }}
        >
          Login
        </Button>
      </Flex>
    </Box>
  );
}
