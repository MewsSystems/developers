import { describe, it } from 'vitest'
import { screen } from '@testing-library/react'
import NavBar from './NavBar'
import { renderWithContext } from '@/tests/renderWithContext'

describe('NavBar', () => {
	it('renders the App component', () => {
		renderWithContext(<NavBar />)
		screen.debug()
	})

	//it('has "Trending Movies" link', () => {
	//	renderWithContext(<NavBar />)
	//	const link = screen.getByLabelText('Trending Movies')
	//})
})