import type { ComponentPropsWithRef } from "react";
import { StyledButton } from "./Button.internal";
import type React from "react";

interface ButtonProps extends ComponentPropsWithRef<"button"> {
  $isCircle: boolean;
  children: React.ReactNode;
}

export default function Button(props: ButtonProps) {
  return (
    <>
      <StyledButton {...props} $isCircle={props.$isCircle}>
        {props.children}
      </StyledButton>
    </>
  );
}
