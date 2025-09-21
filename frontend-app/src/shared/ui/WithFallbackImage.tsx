import { Image, Flex } from "@chakra-ui/react";
import { FaImage } from "react-icons/fa";

export function WithFallbackImage({
  src,
  width,
  height = "169px",
}: {
  src?: string;
  width: string;
  height?: string;
}) {
  if (!src || src === "") {
    return (
      <Flex
        width={width}
        height={height}
        justifyContent={"center"}
        alignItems="center"
      >
        <FaImage size="200px" />
      </Flex>
    );
  }
  return <Image src={src} />;
}
