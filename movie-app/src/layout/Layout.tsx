import { Outlet, useNavigate } from 'react-router-dom'

import { Fab } from '@mui/material'

import { paths } from '@/paths.ts'

const Layout = () => {
    const navigate = useNavigate()

    return (
        <>
            <Fab
                aria-label="Navigate home"
                size="small"
                onClick={() =>
                    navigate(paths.homepage, {
                        replace: true,
                        state: { redirectedPage: 'homepage' },
                    })
                }
                sx={{
                    fontSize: '1.5rem',
                    position: 'absolute',
                    top: 10,
                    right: 10,
                }}
            >
                <span aria-hidden>🏠</span>
            </Fab>
            <Outlet />
        </>
    )
}

export default Layout
