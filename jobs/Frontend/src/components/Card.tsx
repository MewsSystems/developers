import type { PropsWithChildren } from "react";
import { Link } from "react-router-dom";
import styled from "styled-components";
import screen from "../common/screen";
import Image from "../components/Image";

const Wrapper = styled(Link)`
	display: flex;
	flex-direction: column;
	overflow: hidden;
	padding: 0.25rem;
	background-color: rgba(242, 242, 242, 0.7);
	border-radius: 0.625rem;
	text-decoration: none;
	color: inherit;
	transition: box-shadow 0.1s ease;
	box-shadow: 0px 0px 0.5rem 0.25rem rgba(0, 0, 0, 0.07);

	&:hover,
	&:focus {
		box-shadow: 0px 0px 0.75rem 0.5rem rgba(0, 0, 0, 0.15);
	}
`;

const Content = styled.div`
	display: flex;
	flex-direction: column;
	padding: 0.25rem;
`;

const Title = styled.span`
	font-weight: 500;
	color: #333333;
	font-size: 0.875rem;
	line-height: 1.1;
	display: -webkit-box;
	-webkit-box-orient: vertical;
	-webkit-line-clamp: 2;
	text-overflow: ellipsis;
	white-space: pre-line;
	overflow: hidden;
`;

type CardProps = {
	title: string;
	href: string;
	imageUrl?: string;
};

export function Card({ title, href, imageUrl }: CardProps) {
	return (
		<Wrapper to={href}>
			{imageUrl ? <Image rounded src={imageUrl} alt="" role="presentation" /> : null}
			<Content>
				<Title>{title}</Title>
			</Content>
		</Wrapper>
	);
}

const Container = styled.div`
	margin-block: 2rem;
	display: grid;
	gap: 1rem;
	grid-template-columns: repeat(2, 1fr);

	@media ${screen.SM} {
		grid-template-columns: repeat(3, 1fr);
	}

	@media ${screen.M} {
		grid-template-columns: repeat(auto-fill, 180px);
	}
`;

export function CardContainer({ children }: PropsWithChildren) {
	return <Container>{children}</Container>;
}
