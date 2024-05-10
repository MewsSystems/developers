import { Search } from "lucide-react";
import { useState } from "react";
import { useDebounce } from "react-use";
import styled from "styled-components";

const Wrapper = styled.form`
  position: relative;
  width: 100%;
`;

const Input = styled.input`
  width: 100%;
  padding: 10px 20px;
  max-height: 60px;
  border-color: #888;
  border-radius: 18px;
  border-style: solid;
  border-width: 5px;
  margin-top: 15px;
  color: #333;
  font-family: "Helvetica", sans-serif;
  font-size: 26px;
  appearance: none;
`;

const SearchIcon = styled(Search)`
    position: absolute;
	right: 30px;
	top: 30px;
	display: block;
`

type DebouncedSearchInputProps = {
    onSearchChange: (search: string) => void;
    placeholder?: string;
    delay?: number;
    disabled?: boolean;
};

export function DebouncedSearchInput({
    onSearchChange,
    placeholder = "Search...",
    delay = 300,
    disabled = false,
}: DebouncedSearchInputProps) {
    const [searchValue, setSearchValue] = useState("");

    useDebounce(() => onSearchChange(searchValue), delay, [searchValue]);

    return (
        <Wrapper>
            <Input
                type="search"
                placeholder={placeholder}
                value={searchValue}
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                    setSearchValue(e.target.value)
                }
                disabled={disabled}
            />
            {!searchValue && <SearchIcon />}
        </Wrapper>
    );
}
