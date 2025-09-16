import { Box } from "@chakra-ui/react"

export function OpacityBackgroundBox({ children, bgImage, opacity }: React.PropsWithChildren & { bgImage: string, opacity?: number }) {
    return (<Box
        display="flex"
        position="relative"
        width="100%"
        _before={{
            content: '""',
            position: "absolute",
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            backgroundSize: 'cover',
            backgroundPosition: 'center',
            bgImage: bgImage,
            opacity: opacity ?? 0.2,
            zIndex: -1
        }}>
        {children}
    </Box>)
}
