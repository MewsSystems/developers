'use client';

import { useEffect, useState, useMemo } from 'react';
import debounce from 'lodash.debounce';

interface DebouncedInputProps {
  value: string;
  onChange: (value: string) => void;
  debounceDelay?: number;
  placeholder?: string;
  className?: string;
  ariaLabel?: string;
  ariaDescribedBy?: string;
}

export function DebouncedInput({
  value,
  onChange,
  debounceDelay = 500,
  placeholder,
  className,
  ariaLabel,
  ariaDescribedBy,
}: DebouncedInputProps) {
  const [internalValue, setInternalValue] = useState(value);

  useEffect(() => {
    setInternalValue(value);
  }, [value]);

  const debounced = useMemo(
    () => debounce((v: string) => onChange(v), debounceDelay),
    [onChange, debounceDelay]
  );

  useEffect(() => {
    return () => {
      debounced.cancel();
    };
  }, [debounced]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const val = e.target.value;
    setInternalValue(val);
    debounced(val);
  };

  return (
    <input
      type="text"
      placeholder={placeholder}
      value={internalValue}
      onChange={handleChange}
      className={`border border-cyan-800 bg-white p-2 flex-1 rounded ${className}`}
      aria-label={ariaLabel}
      aria-describedby={ariaDescribedBy}
    />
  );
}
