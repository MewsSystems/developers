import styled from "styled-components";
import Poster from "../Poster";

const CardContainer = styled.div<{
	$size: number;
}>`
	--spacing-base: ${({ theme }) => theme.spacing(1)};
	--spacing-multiplier: ${({ $size }) => $size};

	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: space-between;
	border: 1px solid ${({ theme }) => theme.palette.grey[300]};
	border-radius: ${({ theme }) => theme.borderRadius.regular};
	background-color: ${({ theme }) => theme.palette.background.paper};
	box-shadow: ${({ theme }) => theme.shadows.default};

	padding: calc(var(--spacing-base) * var(--spacing-multiplier));
	gap: calc(var(--spacing-base) * var(--spacing-multiplier) * 0.5);
`;

const Body = styled.div<{
	$layout: "column" | "row";
}>`
	display: flex;
	flex-direction: ${({ $layout }) =>
		$layout}; /* Poster and Content in row or column */
	align-items: center;
	gap: ${({ theme }) => theme.spacing(2)};
	margin-bottom: ${({ theme }) => theme.spacing(2)};
`;

const Content = styled.div`
	flex: 1;
	display: flex;
	flex-direction: column;
`;

const Footer = styled.div`
	display: flex;
	justify-content: center;
	align-items: center;
	width: 100%;
`;
interface BaseCardProps {
	size?: number;
	children: React.ReactNode;
}

const BaseCard = ({ size = 1, children }: BaseCardProps) => {
	return (
		<CardContainer data-testid="card-container" $size={size}>
			{children}
		</CardContainer>
	);
};

BaseCard.Poster = Poster;
BaseCard.Content = Content;
BaseCard.Footer = Footer;
BaseCard.Body = Body;

export default BaseCard;
