import { RouterProvider, createRouter } from '@tanstack/react-router'
import { render } from '@testing-library/react'

import { routeTree } from '@/routeTree.gen'

export function renderWithContext(component: React.JSX.Element) {
	console.log(component)
	const router = createRouter({ routeTree })
	return render(<RouterProvider router={router} />)
}