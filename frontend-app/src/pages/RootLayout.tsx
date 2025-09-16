import { Link, Outlet } from '@tanstack/react-router'

export const RootLayout = () => (
    <>
        <div className="p-2 flex gap-2">
            <Link to="/" className="[&.active]:font-bold">
                Home
            </Link>{' '}
        </div>
        <hr />
        <Outlet />
    </>
)
