import { Link } from '@tanstack/react-router'
export function MainPageRouteComponent() {
    return <div>
        <Link to="/movies" className="[&.active]:font-bold">
            List movies
        </Link>{' '}
    </div>
}
