import { Outlet, useNavigate } from 'react-router-dom'

import { Fab } from '@mui/material'

const Layout = () => {
    const navigate = useNavigate()

    return (
        <>
            <Fab
                aria-label="Navigate home"
                size="small"
                onClick={() =>
                    navigate('/', {
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
