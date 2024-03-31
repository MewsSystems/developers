import React from 'react'
import ReactDOM from 'react-dom/client'
import Welcome from './ui/views/Welcome.tsx'
import BaseLayout from './ui/layouts/BaseLayout.tsx'
import { createBrowserRouter, RouterProvider } from 'react-router-dom'

const router = createBrowserRouter([
    {
        path: '/',
        element: <Welcome />,
    },
])

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <BaseLayout>
            <RouterProvider router={router} />
        </BaseLayout>
    </React.StrictMode>
)
