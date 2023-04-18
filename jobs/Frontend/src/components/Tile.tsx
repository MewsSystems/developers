import styled from "styled-components";

const Wrapper = styled.div`
	border-radius: 4px;
	background-color: #efefef;
	color: #333;
	padding: 0.25rem 0.5rem;
	display: inline-flex;
	flex-direction: column;
	line-height: 100%;
	justify-content: center;
	align-items: center;
`;

const Title = styled.span`
	font-size: 0.875rem;
`;

const Subtitle = styled.span`
	font-size: 0.75rem;
	font-style: italic;
`;

type TileProps = {
	title: string;
	subtitle?: string;
};
export function Tile({ title, subtitle }: TileProps) {
	return (
		<Wrapper>
			<Title>{title}</Title>
			{subtitle && <Subtitle>{subtitle}</Subtitle>}
		</Wrapper>
	);
}

export const TileContainer = styled.div`
	display: flex;
	flex-wrap: wrap;
	gap: 0.5rem;
`;
