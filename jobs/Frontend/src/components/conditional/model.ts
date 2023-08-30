import { PropsWithChildren } from "react";

export interface Props extends PropsWithChildren {
    readonly showIf: boolean;
}