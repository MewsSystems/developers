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
                path='/visit-detail/:visitId'
                element={<MovieDetailPage />}
            />
        </Routes>
    )
}
