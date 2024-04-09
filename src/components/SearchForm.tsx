import { useEffect, useState } from "react";
import styled from "styled-components";
import { SearchFormProps } from "../interfaces";

const SectionContainer = styled.section`
    background-color: var(--sand);

    input {
        font-family: "Axiforma-Light", sans-serif;
        min-width: 300px;
    }
`;

export const SearchForm = ({ handleSearch, term }: SearchFormProps) => {
    const [inputValue, setInputValue] = useState("");

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.value.trim() === "") {
            setInputValue("");
            handleSearch("");
            return;
        }
        setInputValue(e.target.value);
        handleSearch(e.target.value.trim());
    };

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
    };

    useEffect(() => {
        setInputValue(term);
    }, [term]);

    return (
        <SectionContainer className="flex flex-col items-center py-8" role="search">
            <form
                onSubmit={handleSubmit}
                className="flex justify-center items-center"
                data-testid="search-form"
            >
                <input
                    type="search"
                    placeholder="Search movies ..."
                    value={inputValue}
                    onChange={handleChange}
                    className="py-2 px-4 rounded-md border border-gray-300 shadow-sm focus:outline-none focus:ring focus:border-blue-300"
                />
            </form>
            {inputValue.trim().length === 0 ? (
                <p className="mt-2 text-xs text-gray-500">Please write a search term.</p>
            ) : null}
        </SectionContainer>
    );
};
