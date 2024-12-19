// TODO: Suggestions for Future Improvements
// 1. Color Customization. Introduce a variant prop to control the input's appearance.
// 2. Size Customization. Introduce a size prop to use conditional styling to adjust the padding and font size.
// 3. Error Handling. Add hasError and errorMessage props to handle error state. Apply error styles and display error message.
// 4. Conditional Label Behavior. Display the label only if there is no placeholder provided or if the input has a value (i.e., the placeholder should be hidden when a value is present).

import styled from "styled-components";

const Wrapper = styled.div`
	display: flex;
	flex-direction: column;
	align-items: flex-start;

	&:focus-within label {
		color: ${({ theme }) => theme.palette.primary.main};
	}
`;

const StyledLabel = styled.label`
	${({ theme }) => theme.typography.body1};
	color: ${({ theme }) => theme.palette.grey[400]};
	margin-left: ${({ theme }) => theme.spacing(0.5)};
	margin-bottom: ${({ theme }) => theme.spacing(0.5)};
	transition: color 0.3s;
`;

const StyledInput = styled.input<{ disabled?: boolean }>`
	width: 100%;
	${({ theme }) => theme.typography.body1};
	padding: ${({ theme }) => theme.spacing(1)};
	color: ${({ theme }) => theme.palette.primary.contrastText};
	border: 1px solid
		${({ theme, disabled }) =>
			disabled ? theme.palette.grey[300] : theme.palette.grey[700]};
	border-radius: ${({ theme }) => theme.borderRadius.small};
	background-color: ${({ theme, disabled }) =>
		disabled ? theme.palette.grey[100] : theme.palette.background.dark};
	outline: none;
	transition: border-color 0.3s, background-color 0.3s;

	&:focus {
		border-color: ${({ theme }) => theme.palette.primary.main};
	}

	&:disabled {
		cursor: not-allowed;
	}

	::placeholder {
		color: ${({ theme }) => theme.palette.grey[400]};
	}
`;

interface InputProps {
	id: string;
	name: string;
	value: string;
	onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
	label?: string;
	placeholder?: string;
	type?: string;
	ariaLabel?: string;
	disabled?: boolean;
}

const Input = ({
	id,
	name,
	value,
	onChange,
	label,
	placeholder,
	type = "text",
	ariaLabel = placeholder,
	disabled = false,
}: InputProps) => (
	<Wrapper>
		{label && <StyledLabel htmlFor={id}>{label}</StyledLabel>}
		<StyledInput
			id={id}
			name={name}
			type={type}
			value={value}
			onChange={onChange}
			placeholder={!label ? placeholder : ""}
			aria-label={ariaLabel}
			disabled={disabled}
		/>
	</Wrapper>
);

export default Input;
