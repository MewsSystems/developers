import styled, { css } from "styled-components";

const variants = {
	h1: css`
		${({ theme }) => theme.typography.h1};
		color: ${({ theme }) => theme.palette.text.primary};
	`,
	h2: css`
		${({ theme }) => theme.typography.h2};
		color: ${({ theme }) => theme.palette.text.primary};
	`,
	h3: css`
		${({ theme }) => theme.typography.h3};
		color: ${({ theme }) => theme.palette.text.primary};
	`,
	h4: css`
		${({ theme }) => theme.typography.h4};
		color: ${({ theme }) => theme.palette.text.secondary};
	`,
	h5: css`
		${({ theme }) => theme.typography.h5};
		color: ${({ theme }) => theme.palette.text.secondary};
	`,
	h6: css`
		${({ theme }) => theme.typography.h6};
		color: ${({ theme }) => theme.palette.text.secondary};
	`,
	subtitle: css`
		${({ theme }) => theme.typography.body1};
		color: ${({ theme }) => theme.palette.text.secondary};
	`,
	body: css`
		${({ theme }) => theme.typography.body2};
		color: ${({ theme }) => theme.palette.text.secondary};
	`,
};

interface TypographyProps {
	variant?: "subtitle" | "body" | "h1" | "h2" | "h3" | "h4" | "h5" | "h6";
	color?: string;
	fontWeight?: number | string;
	fontSize?: string;
	textAlign?: string;
	as?: keyof JSX.IntrinsicElements;
}

const Typography = styled.p<TypographyProps>`
	margin: 0;
	${({ variant = "body" }) => variants[variant]};
	color: ${({ color, theme, variant }) =>
		color || theme.typography[variant || "body"]?.color};
	font-weight: ${({ fontWeight }) => fontWeight};
	font-size: ${({ fontSize }) => fontSize};
	text-align: ${({ textAlign }) => textAlign || "center"};
`;

const forwardProps: (keyof TypographyProps)[] = [
	"variant",
	"color",
	"fontWeight",
	"fontSize",
	"textAlign",
	"as",
];

const TypographyWithConfig = styled(Typography).withConfig({
	shouldForwardProp: (prop) =>
		!forwardProps.includes(prop as keyof TypographyProps),
})``;

export default TypographyWithConfig;
