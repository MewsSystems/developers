import React, { useState } from "react";
import { Input } from "@/components/ui/input";

type DebouncedInputProps = {
  debouncedOnChange: (value: string) => void;
  delay?: number;
  delayedThreshold?: number;
  initialValue?: string;
} & React.ComponentProps<typeof Input>;
const DebouncedInput = ({
  debouncedOnChange,
  initialValue,
  delay = 500,
  delayedThreshold = 7,
  ...props
}: DebouncedInputProps) => {
  const [timeout, setTimeoutState] = useState<NodeJS.Timeout | null>(null);
  const [inputValue, setInputValue] = useState<string>(initialValue ?? "");
  const [clearedCount, setClearedCount] = useState<number>(0);

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    props.onChange?.(e);
    const value = e.target.value;

    setInputValue(value);

    if (timeout) {
      if (clearedCount + 1 >= delayedThreshold) {
        clearTimeout(timeout);
        setClearedCount(0);
        debouncedOnChange(value);
        return;
      }

      clearTimeout(timeout);
      setClearedCount(clearedCount + 1);
    }
    if (!value) {
      debouncedOnChange(value);
      setClearedCount(0);
      return;
    }

    setTimeoutState(
      setTimeout(() => {
        debouncedOnChange(value);
      }, delay),
    );
  };
  return <Input {...props} onChange={onChange} value={inputValue} />;
};

export default DebouncedInput;
