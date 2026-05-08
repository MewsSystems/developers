// TODO: Suggestions for Future Improvements
// 1. Color Customization. Introduce a variant prop to control the button's appearance.
// 2. Size Customization. Introduce a size prop to use conditional styling to adjust the padding and font size.
// 3. Full-width. Introduce a fullWidth prop to optionally make the button take up the full width of its container.
// 4. Add a loading state for when the button is clicked.

import React from "react";
import styled from "styled-components";

const StyledButton = styled.button<{ disabled?: boolean }>`
	${({ theme }) => theme.typography.button};
	background-color: ${({ disabled, theme }) =>
		disabled ? theme.palette.grey[400] : theme.palette.primary.main};
	color: ${({ disabled, theme }) =>
		disabled
			? theme.palette.text.disabled
			: theme.palette.primary.contrastText};
	padding: ${({ theme }) => `${theme.spacing(1)} ${theme.spacing(3)}`};
	margin: ${({ theme }) => theme.spacing(1)};
	border: none;
	border-radius: ${({ theme }) => theme.borderRadius.small};
	cursor: ${({ disabled }) => (disabled ? "not-allowed" : "pointer")};
	outline: none;
	transition: background-color 0.3s, transform 0.2s;

	&:hover {
		background-color: ${({ disabled, theme }) =>
			disabled ? theme.palette.grey[400] : theme.palette.primary.dark};
		transform: ${({ disabled }) => (disabled ? "none" : "scale(1.05)")};
	}

	&:focus {
		box-shadow: ${({ theme }) => theme.shadows.focus};
	}
`;

interface ButtonProps {
	onClick: () => void;
	children: React.ReactNode;
	disabled?: boolean;
	ariaLabel?: string;
}

const Button = ({
	onClick,
	children,
	disabled = false,
	ariaLabel,
}: ButtonProps) => {
	return (
		<StyledButton
			onClick={onClick}
			disabled={disabled}
			aria-label={ariaLabel}
			aria-disabled={disabled}
		>
			{children}
		</StyledButton>
	);
};

export default React.memo(Button);
