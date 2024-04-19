import ReactDOM from 'react-dom/client';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import App from './App';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MovieDetail } from './components/views/MovieDetail/MovieDetail';
import Movies from './components/views/Movies/Movies';
import { GlobalStyle } from './main.styled';
import { NotFound } from './components/views/NotFound';

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
            {index: true, element: <Movies/>},
            {path: '/movie/:movieId', element: <MovieDetail/>}
        ]
    },
]);

root.render(
    <QueryClientProvider client={queryClient}>
        <GlobalStyle/>
        <RouterProvider router={router}/>
    </QueryClientProvider>
);
