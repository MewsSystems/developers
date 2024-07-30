import { Outlet, createRootRoute } from '@tanstack/react-router'
import styled from 'styled-components'
import { Link } from '@tanstack/react-router'
import { memo } from 'react'

import { routeNames } from '@/constants/routeNames'
import Layout from '@/components/Layout'

const NotFoundComponent = styled.h1`
	text-align: center;
	margin-top: 50px;
`

const HomeLink = styled(Link)`
	background: #04AA6D;
	color: #fff;
	padding: 10px 20px;
	border-radius: 10px;
	margin-top: 20px;
	display: inline-block;
	text-decoration: none;
`

function Root() {
	return (
		<Layout>	
			<Outlet />
		</Layout>
	)
}

export const Route = createRootRoute({
	component: memo(Root),
	notFoundComponent: () => (
		<NotFoundComponent>
			Page not found: 404 <br />
			<HomeLink to={routeNames.index}>Back to Home Page</HomeLink>
		</NotFoundComponent>
	)
})