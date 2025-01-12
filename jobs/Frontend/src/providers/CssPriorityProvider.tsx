import { StyledEngineProvider } from "@mui/material";
import { PropsWithChildren } from "react";

export const CssPriorityProvider = ({ children }: PropsWithChildren) => {
    return <StyledEngineProvider injectFirst>{children}</StyledEngineProvider>;
};
