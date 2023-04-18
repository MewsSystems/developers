import styled from "styled-components";
import screen from "../common/screen";
import { GoChevronLeft as IconLeft } from "react-icons/go";
import { GoChevronRight as IconRight } from "react-icons/go";

const _Button = styled.button<{ iconEnd: boolean }>`
	background-color: hsl(0, 0%, 90);
	border: 0.125rem solid #ba846d;
	border-radius: 0.625rem;
	box-shadow: 0px 0px 0.5rem 0.25rem rgba(0, 0, 0, 0.07);
	display: inline-flex;
	justify-content: center;
	align-items: center;
	gap: 0.5rem;
	line-height: 1;
	color: inherit;
	cursor: pointer;
	text-decoration: none;
	font-size: 0.875rem;
	font-weight: bold;
	transition: background-color 0.2s;
	flex-direction: ${(props) => (props.iconEnd ? "row-reverse" : "row")};
	padding: 0.25rem 0.5rem;

	&[disabled] {
		opacity: 0.5;
	}

	&:hover:not([disabled]),
	&:focus {
		background-color: #e79f7f;
		box-shadow: 0px 0px 0.25rem 0.25rem rgba(0, 0, 0, 0.1);
	}

	svg {
		height: 1.5rem;
		width: 1.5rem;
	}

	@media ${screen.SM} {
		padding: 0.5 1rem;
	}
`;

type ButtonProps = {
	variant?: "Default" | "Back" | "Forward";
	disabled?: boolean;
	text: string;
	onClick: () => void;
};
export default function Button({ variant = "Default", text, disabled = false, onClick }: ButtonProps) {
	let icon = null;
	if (variant !== "Default") {
		icon = variant === "Back" ? <IconLeft /> : <IconRight />;
	}

	return (
		<_Button disabled={disabled} iconEnd={variant === "Forward"} onClick={onClick}>
			{icon}
			{text}
		</_Button>
	);
}
