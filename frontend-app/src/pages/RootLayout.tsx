import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import {
  Box,
  Button,
  Flex,
  HStack,
  IconButton,
  useDisclosure,
  Stack,
} from "@chakra-ui/react";
import {
  Outlet,
  useNavigate,
  useRouter,
  Link,
  useMatch,
} from "@tanstack/react-router";
import { Avatar } from "@chakra-ui/react";
import useQueryAccount from "@/entities/account/hooks/useQueryAccount";
import { GiHamburgerMenu } from "react-icons/gi";
import useQueryAccountFavoriteMovies from "@/entities/account/hooks/useQueryAccountFavoriteMovies";
import useQueryAccountWatchListMovies from "@/entities/account/hooks/useQueryAccountWatchListMovies";

export const RootLayout = () => {
  const router = useRouter();
  const navigate = useNavigate();
  const auth = useAuth();

  const { data: dataAccountFavoritesMovies } = useQueryAccountFavoriteMovies({
    accountId: auth?.accountId ?? 0,
  });

  const { data: dataAccountsWatchListMovies } = useQueryAccountWatchListMovies({
    accountId: auth?.accountId ?? 0,
  });

  const matchMovies = useMatch({ from: "/_auth/movies", shouldThrow: false });
  const matchRoot = useMatch({ from: "/", shouldThrow: false });

  const links = [
    { name: "Home", to: "/", selected: matchRoot !== undefined },
    {
      name: "Search movies",
      to: "/movies",
      auth: true,
      selected: matchMovies !== undefined,
    },
  ].filter((link) => !link.auth || (link.auth && auth.isAuthenticated));

  const { open, onOpen, onClose } = useDisclosure();

  const handleLogout = () => {
    if (window.confirm("Are you sure you want to logout?")) {
      auth.logout().then(() => {
        router.invalidate().finally(() => {
          navigate({ to: "/" });
        });
      });
    }
  };

  return (
    <Flex direction={"column"} height={"100vh"}>
      <Box px={4}>
        <Flex h={16} alignItems={"center"} justifyContent={"space-between"}>
          <IconButton
            size={"md"}
            aria-label={"Open Menu"}
            display={{ md: "none" }}
            onClick={open ? onClose : onOpen}
          >
            <GiHamburgerMenu />
          </IconButton>
          <HStack gap={8} alignItems={"center"}>
            <HStack as={"nav"} gap={4} display={{ base: "none", md: "flex" }}>
              {links.map((link) => (
                <NavLink
                  key={link.name}
                  to={link.to}
                  name={link.name}
                  selected={link.selected}
                />
              ))}
            </HStack>
          </HStack>
          <AuthenticatedComponents>
            <Flex alignItems={"center"} gap="4">
              <Box>Favorites({dataAccountFavoritesMovies?.total_results})</Box>
              <Box>Watchlist({dataAccountsWatchListMovies?.total_results})</Box>

              <AvatarItem />
              <Button
                variant={"solid"}
                colorScheme={"teal"}
                size={"sm"}
                mr={4}
                onClick={() => {
                  handleLogout();
                }}
              >
                Log out
              </Button>
            </Flex>
          </AuthenticatedComponents>
        </Flex>

        {open ? (
          <Box display={{ md: "none" }}>
            <Stack as={"nav"} gap={4}>
              {links.map((link) => (
                <NavLink key={link.name} to={link.to} name={link.name} />
              ))}
            </Stack>
          </Box>
        ) : null}
      </Box>

      <Box p={4} height={"100%"}>
        <Outlet />
      </Box>
    </Flex>
  );
};

function AuthenticatedComponents({ children }: React.PropsWithChildren) {
  const auth = useAuth();
  if (!auth.isAuthenticated) {
    return <></>;
  }
  return <>{children}</>;
}

function AvatarItem() {
  const { data: account } = useQueryAccount();
  return (
    <Avatar.Root>
      <Avatar.Fallback name={account?.username} />
    </Avatar.Root>
  );
}

interface Props {
  name: string;
  to: string;
  selected?: boolean;
}

const NavLink = (props: Props) => {
  const { name, to, selected } = props;

  return (
    <Box
      px={2}
      py={1}
      rounded={"md"}
      borderColor={selected ? "black" : ""}
      borderWidth={"1px"}
      _hover={{
        textDecoration: "none",
        bg: "grey",
      }}
    >
      <Link to={to}>{name}</Link>
    </Box>
  );
};
