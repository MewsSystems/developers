import { createFileRoute, Link } from '@tanstack/react-router'

export const Route = createFileRoute("/" as any)({
    component: MainPage,
})

function MainPage() {
    return <div>
        <Link to="/movies" className="[&.active]:font-bold">
            List movies
        </Link>{' '}
    </div>
}