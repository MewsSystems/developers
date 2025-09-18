import { useEffect, useState } from "react";
import styled from "styled-components";
import { Input } from "@/shared/ui/atoms/Input";
import { useDebouncedValue } from "@/shared/hooks/useDebouncedValue";

const Wrap = styled.div`
  margin: 2rem;
`;

type SearchInputProps = {
  initialValue: string;
  onChange: (v: string) => void;
};

export function SearchInput({ initialValue, onChange }: SearchInputProps) {
  const [value, setValue] = useState(initialValue);
  const debounced = useDebouncedValue(value);

  useEffect(() => {
    onChange(debounced.trim().toLocaleLowerCase());
  }, [debounced, onChange]);

  return (
    <Wrap>
      <Input
        type="text"
        name="q"
        value={value}
        placeholder="Search for a movie..."
        onChange={(e) => setValue(e.target.value)}
        aria-label="Search movies"
        autoComplete="off"
      />
    </Wrap>
  );
}
