import type { ComponentPropsWithRef } from "react";
import { StyledImage } from "./Image.internal";
import type { CSSProperties } from "styled-components";

interface ImageProps extends ComponentPropsWithRef<"img"> {
  $width: CSSProperties["width"];
}

export const Image = (props: ImageProps) => {
  return (
    <>
      <StyledImage {...props} $width={props.$width} />
    </>
  );
};
