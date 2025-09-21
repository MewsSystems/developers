import { Spinner, VStack, Text, Box } from "@chakra-ui/react";

export default function LoadingSpinner() {
  return (
    <VStack colorPalette="teal" height={"100%"} justifyContent={"center"}>
      <Spinner color="teal.500" size="xl" />
      <Text color="teal.500">Loading...</Text>
    </VStack>
  );
}
