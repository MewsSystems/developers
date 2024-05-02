import { useState, ChangeEvent } from 'react'
import styled, { keyframes } from 'styled-components'
import throttle from 'lodash/throttle'
import { useNavigate, useRouterState } from '@tanstack/react-router'
import { IoSearchCircleSharp } from 'react-icons/io5'
import { FaSpinner } from 'react-icons/fa'

import { useMovies } from '@/state/movies'
import { navBarHeight } from './NavBar'

export const inputBlockHeight = 76

const Wrapper = styled.div`
	position: fixed;
	top: ${navBarHeight}px;
	height: ${inputBlockHeight}px;
	box-sizing: border-box;
	padding: 10px 20px;
	position: fixed;
	width: 100%;
	display: flex;
	justify-content: center;
	background: #fff;
	z-index: 1;
`

const InputBlock = styled.div`
	max-width: 900px;
	width: 100%;
	position: relative;
`

const InputIcon = styled.div`
	position: absolute;
	left: 10px;
	top: 0;
	bottom: 0;
	display: flex;
	align-items: center;
	justify-content: center;
`

const Input = styled.input`
	border-radius: 10px;
	height: 50px;
	width: 100%;
	padding-left: 50px;
	font-size: 2em;
	box-sizing: border-box;
`

const rotate = keyframes`
	from {
		transform: rotate(0deg);
	}

	to {
		transform: rotate(360deg);
	}
`

const Spinner = styled(FaSpinner)`
	animation-name: ${rotate};
	animation-duration: 2000ms;
	animation-iteration-count: infinite;
	animation-timing-function: linear;
`

export default function NavBar() {
	//const { query } = Route.useSearch()
	const router = useRouterState()
	const [searchQuery, setSearchQuery] = useState(router.location.search.query)
	const navigate = useNavigate({ from: router.location.pathname })
	const { status } = useMovies()

	const throttledNavigate = throttle(navigate, 500, { leading: false, trailing: true })

	function onChange(e: ChangeEvent<HTMLInputElement>) {
		setSearchQuery(e.target.value)
		throttledNavigate({ search: () => ({ query: e.target.value }) })
	}

	return (
		<Wrapper>
			<InputBlock>
				<InputIcon>
					{ status === 'pending' ? <Spinner size={'2em'} /> : <IoSearchCircleSharp size={'2em'} />}
				</InputIcon>
				<Input onChange={onChange} value={searchQuery} />
			</InputBlock>
		</Wrapper>
	)
}