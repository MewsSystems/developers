import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { Button, Flex, Text } from "@chakra-ui/react";

export function Login() {
    const auth = useAuth();
    return <div>
        <Flex direction="column" justifyContent="center" justifyItems="items" alignContent="center" alignItems="center">
            <Text>
                Login page
            </Text>
            <Button width="50%" onClick={() => {
                auth.login();
            }}>Login</Button>
        </Flex>
    </div>
}