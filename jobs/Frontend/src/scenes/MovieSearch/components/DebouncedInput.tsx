"use client";

import React, { useState } from "react";
import { Input } from "@/components/ui/input";

type DebouncedInputProps = {
  debouncedOnChange: (value: string) => void;
  delay?: number;
  delayedThreshold?: number;
  initialValue?: string;
} & React.ComponentProps<typeof Input>;

/**
 * Client-side input component which supports debouncing the onChange event.
 *
 * @param debouncedOnChange - function to be called after the debouncing delay
 * @param initialValue - initial value of the input
 * @param delay - delay in ms after which the debouncedOnChange function is called
 * @param delayedThreshold - number of times the timeout can be cleared before the debouncedOnChange function is called
 * @param props - other props for {@link Input} component
 * @constructor
 */
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
      clearTimeout(timeout);

      // If the threshold is reached, call the debouncedOnChange function immediately
      if (clearedCount + 1 >= delayedThreshold) {
        setClearedCount(0);
        debouncedOnChange(value);
      } else {
        setClearedCount(clearedCount + 1);
      }
    }

    // If the input is empty, call the debouncedOnChange function immediately
    if (!value) {
      debouncedOnChange(value);
      setClearedCount(0);
      return;
    }

    // Call the debouncedOnChange function after the delay
    setTimeoutState(
      setTimeout(() => {
        debouncedOnChange(value);
      }, delay),
    );
  };
  return <Input {...props} onChange={onChange} value={inputValue} />;
};

export default DebouncedInput;
