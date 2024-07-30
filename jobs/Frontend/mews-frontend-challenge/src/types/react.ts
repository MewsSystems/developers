import React from "react";

export type BaseComponentProps = React.PropsWithChildren<{
  className?: string;
  "aria-label"?: string;
}>;
