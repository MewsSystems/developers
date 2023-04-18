import { useEffect, useState, useCallback } from "react";
import styled from "styled-components";
import { GoSearch } from "react-icons/go";

const SEARCH_DELAY = 400;

const Field = styled.div`
	min-height: 2rem;
	width: 100%;
	max-width: 20rem;
	border: 0.125rem solid #ba846d;
	border-radius: 0.625rem;
	background-color: #f2f2f2;
	color: #656565;
	display: inline-flex;
	flex-direction: row;
	overflow: hidden;
	transition: box-shadow 0.1s ease;

	&:focus-within {
		border-color: #333333;
		color: #333333;
		box-shadow: 0px 0px 1rem 0.25rem rgba(0, 0, 0, 0.15);
	}
`;

const Input = styled.input`
	flex: 1;
	min-width: 0;
	font-size: 1rem;
	background-color: transparent;
	padding: 0.25rem 0 0.25rem 0.5rem;
	border: none;
	color: inherit;
	font-style: normal;
	appearance: none;
	-webkit-appearance: none;

	&:focus {
		outline: none;
	}
`;

const Button = styled.button`
	display: inline-flex;
	justify-content: center;
	align-items: center;
	border: none;
	padding: 0;
	cursor: pointer;
	width: 2rem;
	appearance: none;
	color: inherit;
	transition: background-color 0.1s, color 0.1s, outline-offset 0.1s;

	&:hover,
	&:focus {
		background-color: #333333;
		color: #f2f2f2;
	}

	&:focus {
		outline-offset: -0.25rem;
	}

	&:active {
		color: #1f1f1f;
		outline-color: transparent;
		background-color: #f2f2f2;
	}
`;

const Icon = styled(GoSearch)`
	width: 1.25rem;
	height: 1.25rem;
`;

type SearchFieldProps = {
	placeholder: string;
	onSearch: (value: string) => void;
};

export default function SearchField({ placeholder, onSearch }: SearchFieldProps) {
	const [value, setValue] = useState("");

	const triggerSearch = useCallback(() => {
		if (value.trim()) {
			onSearch(value);
		}
	}, [value, onSearch]);

	useEffect(() => {
		const timer = setTimeout(triggerSearch, SEARCH_DELAY);

		return () => clearTimeout(timer);
	}, [value, triggerSearch]);

	return (
		<Field>
			<Input
				value={value}
				spellCheck={false}
				placeholder={placeholder}
				onInput={(e: React.ChangeEvent<HTMLInputElement>) => {
					setValue(e.target.value);
				}}
			/>
			<Button aria-label="Go" onClick={triggerSearch}>
				<Icon role="presentation" />
			</Button>
		</Field>
	);
}
