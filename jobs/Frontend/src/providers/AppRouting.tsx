import { Routes, Route } from 'react-router-dom'
import { HomePage, MovieDetailPage } from '../pages'

export const AppRouting = () => {
    return (
        <Routes>
            <Route
                path='/'
                element={<HomePage />}
            />
            <Route
                path='/movie/:id'
                element={<MovieDetailPage />}
            />
        </Routes>
    )
}
