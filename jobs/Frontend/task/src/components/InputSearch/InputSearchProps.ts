import { LegacyRef } from "react";

export interface InputSearchProps {
  onDebounce?: (value: string) => void;
  onChange?: (value: string) => void;
  onEnter?: (value: string) => void;
  debounceTime?: number;
  innerRef?: LegacyRef<HTMLInputElement>;
  placeholder?: string;
  loading?: boolean;
}
