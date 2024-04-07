import { useEffect, useState } from "react";
import { SearchFormProps } from "../interfaces";

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
        <nav>
            <form data-testid="search-form" onSubmit={handleSubmit}>
                <input
                    type="text"
                    placeholder="Search movies ..."
                    value={inputValue}
                    onChange={handleChange}
                />
            </form>
            {inputValue.trim().length === 0 ? <p>Please write a search term.</p> : null}
        </nav>
    );
};
