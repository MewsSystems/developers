import type { ComponentPropsWithRef } from "react";
import { StyledImage } from "./Image.internal";

interface ImageProps extends ComponentPropsWithRef<"img"> {}

export const Image = (props: ImageProps) => {
  return (
    <>
      <StyledImage {...props} />
    </>
  );
};
