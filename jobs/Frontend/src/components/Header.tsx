import { Link } from "react-router-dom";
import styled from "styled-components";
import logo from "../common/assets/logo.svg";

const Image = styled.img`
	display: block;
	pointer-events: none;
`;

const Logo = styled(Link)`
	line-height: 1;
	display: inline-block;
	margin-block-end: 2rem;
`;

const HiddenTitle = styled.h1`
	width: 1px;
	height: 1px;
	padding: 0;
	margin: -1px;
	overflow: hidden;
	clip: rect(0, 0, 0, 0);
	white-space: nowrap;
	border: 0;
`;

export default function Header() {
	return (
		<header>
			<Logo to="/">
				<Image src={logo} alt="" role="presentation" />
				<HiddenTitle>Kino - Movie Search</HiddenTitle>
			</Logo>
		</header>
	);
}
