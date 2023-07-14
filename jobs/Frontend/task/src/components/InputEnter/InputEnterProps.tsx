import { LegacyRef } from "react";

export interface InputEnterProps {
  onChange?: (value: string | number) => void;
  onEnter: (value: string | number) => void;
  innerRef?: LegacyRef<HTMLInputElement>;
  placeholder?: string;
  value: string | number;
  inputType: "number" | "text";
}
