import ReactDOM from 'react-dom/client';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import App from './App.tsx';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MovieDetail } from './components/views/MovieDetail/MovieDetail.tsx';
import Movies from './components/views/Movies/Movies.tsx';
import { GlobalStyle } from './main.styled.tsx';
import { NotFound } from './components/views/NotFound.tsx';

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);

const queryClient = new QueryClient();

const router = createBrowserRouter([
    {
        path: '/',
        element: <App/>,
        errorElement: <NotFound/>,
        children: [
            { index: true, element: <Movies /> },
            { path: '/movie/:movieId', element: <MovieDetail /> }
        ]
    },
]);

root.render(
    <QueryClientProvider client={queryClient}>
        <GlobalStyle />
        <RouterProvider router={router} />
    </QueryClientProvider>
);
