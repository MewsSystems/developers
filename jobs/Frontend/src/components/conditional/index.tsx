import { Props } from "./model";

export const Conditional = (props: Props) => {
    return props.showIf
        ? <>{props.children}</>
        : <></>;
}



