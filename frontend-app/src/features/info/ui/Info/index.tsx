import { Box, Flex, Text } from "@chakra-ui/react";
import { HomepageComponent } from "@/features/info/ui/Info/HomePageLink";
import { KeywordsComponent } from "@/features/info/ui/Info/KeywordsComponent";
import { IMDBLink } from "./ImdbLink";
import type { MovieInfo } from "../../types";

export function Info({ info }: { info: MovieInfo }) {
  return (
    <Flex direction={"column"} gap="4">
      <Flex gap="5">
        {info.imdbLink && <IMDBLink imdbLink={info.imdbLink} />}
        {info.homepage && <HomepageComponent homepage={info.homepage} />}
      </Flex>{" "}
      <Box>
        <Text textStyle={"lg"}>Status</Text>
        <Text>{info.status}</Text>
      </Box>
      <Box>
        <Text textStyle={"lg"}>Original Language</Text>
        <Text>{info.originalLanguage}</Text>
      </Box>
      <Box>
        <Text textStyle={"lg"}>Budget</Text>
        <Text>{info.budget}</Text>
      </Box>
      <Box>
        <Text textStyle={"lg"}>Revenue</Text>
        <Text>{info.revenue}</Text>
      </Box>
      <Box>
        <Text textStyle={"lg"}>Keywords</Text>
        <KeywordsComponent keywords={info.keywords} />
      </Box>
    </Flex>
  );
}
