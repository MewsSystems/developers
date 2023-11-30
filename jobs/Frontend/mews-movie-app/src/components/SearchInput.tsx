import { on } from "events";
import React, { ChangeEvent, useEffect } from "react";
import { useState } from "react";
import styled from "styled-components";

type Props = {
    initialValue?: string;
    placeholder?: string;
    onChange: (value: string) => void;
    ["aria-label"]: string;
};

const Input = styled.input`
    width: 100%;
    max-width: 500px;
    padding: 1rem;
    border: 1px solid #ccc;
    border-radius: 5px;
    font-size: 1rem;
    margin-bottom: 1rem;
`;

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
        }, 1000);
        return () => clearTimeout(timeoutId);
    }, [value, onChange, initialValue]);

    return (
        <Input type="search" value={value} onChange={handleChange} {...props} />
    );
};

export default SearchInput;
