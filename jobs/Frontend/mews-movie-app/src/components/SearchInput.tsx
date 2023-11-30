import { ChangeEvent, useEffect } from "react";
import { useState } from "react";
import styled from "styled-components";
import { FiSearch, FiX } from "react-icons/fi";

const bounceLimit = 500;

const Input = styled.input`
    width: 100%;
    max-width: 500px;
    border: none;
    font-size: 1rem;
    color: #3d3d3d;

    &:focus {
        outline: none;
    }
`;

const Label = styled.label`
    width: 100%;
    display: inline-flex;
    align-items: center;
    padding: 0.5rem;
    gap: 0.5rem;
    border-radius: 999rem;
    background-color: #fff;

    &:focus-within {
        outline: 3px solid var(--focus-color);
    }
`;

const SearchIcon = styled(FiSearch)`
    margin: 0.5rem;
    flex-shrink: 0;
    color: #999;
`;

const ResetButton = styled.button<{ $visible?: boolean }>`
    display: inline-flex;
    align-items: center;
    flex-shrink: 0;
    padding: 0.7rem;
    border: none;
    background: #eee;
    border-radius: 50%;
    cursor: pointer;
    visibility: ${(props) => (props.$visible ? "visible" : "hidden")};

    &:focus-visible {
        outline: 3px solid var(--focus-color);
    }

    &:hover {
        background-color: #ddd;
    }
`;

type Props = {
    initialValue?: string;
    placeholder?: string;
    onChange: (value: string) => void;
    ["aria-label"]: string;
};

const SearchInput = ({ initialValue = "", onChange, ...props }: Props) => {
    const [value, setValue] = useState(initialValue);

    const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
        setValue(event.target.value);
    };

    useEffect(() => {
        const timeoutId = setTimeout(() => {
            if (initialValue !== value) {
                onChange(value);
            }
        }, bounceLimit);
        return () => clearTimeout(timeoutId);
    }, [value, onChange, initialValue]);

    return (
        <form role="search" onReset={() => setValue("")}>
            <Label>
                <SearchIcon size="18" />
                <Input value={value} onChange={handleChange} {...props} />
                <ResetButton type="reset" $visible={value.length > 0}>
                    <FiX />
                </ResetButton>
            </Label>
        </form>
    );
};

export default SearchInput;
