import { Image, Flex } from "@chakra-ui/react"
import { FaImage } from "react-icons/fa";

export function WithFallbackImage({ src, width }: { src?: string, width: string }) {
    if (!src) {
        return <Flex width={width} height="169px" justifyContent={"center"} alignItems="center"><FaImage size="200px" /></Flex>
    }
    return <Image src={src} />
}