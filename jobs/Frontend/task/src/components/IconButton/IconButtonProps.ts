import { ReactNode } from "react";

export interface IconButtonProps {
  name?: ReactNode | string;
  onClick: () => void;
  disabled?: boolean;
}
