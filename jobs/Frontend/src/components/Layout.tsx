import { Outlet } from "react-router-dom";
import styled from "styled-components";
import screen from "../common/screen";
import Header from "../components/Header";
import backgroundImage from "../common/assets/bg.svg";

const Wrapper = styled.div`
	min-height: 100%;
	background-image: url(${backgroundImage});
	background-attachment: fixed;
	background-repeat: no-repeat;
	background-size: cover;
	padding: 1rem;

	@media ${screen.M} {
		padding: 2rem;
	}

	@media ${screen.L} {
		padding: 3rem;
	}
`;

export default function Layout() {
	return (
		<Wrapper>
			<Header />
			<Outlet />
		</Wrapper>
	);
}
