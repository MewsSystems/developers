import styled from 'styled-components'
import { useRouterState } from '@tanstack/react-router'


import { routeNames } from '@/constants/routeNames'
import NavBar, { navBarHeight } from './NavBar'
import SearchInput, { inputBlockHeight } from './SearchInput'

export const maxContentWidth = 900

type Props = {
	children: JSX.Element
}


const Wrapper = styled.div`
	padding-top: ${navBarHeight + inputBlockHeight}px;
	position: relative;
`


export default function Layout(props: Props) {
	const router = useRouterState({ select: state => state.location })
	const isSearch = router.pathname === routeNames.search

	return (
		<Wrapper>
			<NavBar />
			{ isSearch && <SearchInput />}
			{ props.children }
		</Wrapper>
	)
}