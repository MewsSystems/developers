import styled from 'styled-components'
import { Link, useMatches } from '@tanstack/react-router'
import { routeNames } from '@/constants/routeNames'

export const navBarHeight = 50

const Nav = styled.ul`
	position: fixed;
	display: flex;
	justify-content: center;
	left: 0;
	top: 0;
	right: 0;
	list-style-type: none;
	margin: 0;
	padding: 0;
	overflow: hidden;
	background-color: #333;
	color: #fff;
	z-index: 1;
`

const NavItem = styled.div`
	float: left;
	&:nth-child(n+2) {
		&:before {
			content: "|";
			display: inline-block;
		}
	}
`

const NavLink = styled(Link)`
	display: inline-block;
	color: white;
	text-align: center;
	padding: 16px 16px;
	text-decoration: none;

	&.active {
		background-color: #04AA6D;
	}

	&:hover {
		background: #000;
	}
`

export default function NavBar() {
	const matches = useMatches()
	const isMovieRoute = matches.find(route => route.routeId === routeNames.movie)

	return (
		<Nav>
			<NavItem>
				<NavLink to={routeNames.search} search={{ query: '' }}>Trending Movies</NavLink>
			</NavItem>
			{isMovieRoute &&
				<NavItem>
					{<NavLink onClick={() => window.history.back()} to={'###'}>Back To Search</NavLink>}
				</NavItem>
			}
		</Nav>
	)
}