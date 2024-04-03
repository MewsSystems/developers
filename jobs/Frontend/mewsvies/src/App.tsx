import BaseLayout from './ui/layouts/BaseLayout.tsx'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'
import Home from './ui/views/Home.tsx'
import MovieDetail from './ui/views/MovieDetail.tsx'

const router = createBrowserRouter([
    {
        element: <BaseLayout />,
        children: [
            {
                path: '/',
                element: <Home />,
            },
            {
                path: 'movie/:id',
                element: <MovieDetail />,
            },
        ],
    },
])

const App = () => <RouterProvider router={router} />
export default App
