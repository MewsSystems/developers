import { Loader2 } from "lucide-react";

type SpinnerProps = {
    size?: number;
    className?: string;
    "aria-label"?: string;
};

export default function Spinner({
    size = 20,
    className = "",
    ...rest
}: SpinnerProps) {
    return (
        <Loader2
            className={`animate-spin ${className}`}
            width={size}
            height={size}
            aria-hidden={rest["aria-label"] ? undefined : true}
            {...rest}
        />
    );
}
