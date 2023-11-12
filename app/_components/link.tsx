import { FC, ReactNode } from "react";
import { LinkProps as NextLinkProps } from "next/link";
import { Link as NextLink } from "@/navigation/index";
import { Link as StyledLink } from "@/styles/base/link";

type Props = NextLinkProps & {
  variant?: "primary" | "secondary";
  children: ReactNode;
};

export const Link: FC<Props> = ({
  children,
  variant = "primary",
  locale,
  ...restProps
}) => {
  return (
    <NextLink {...restProps} style={{ textDecoration: "none" }}>
      <StyledLink $variant={variant} $size="lg" $fw={600}>
        {children}
      </StyledLink>
    </NextLink>
  );
};
