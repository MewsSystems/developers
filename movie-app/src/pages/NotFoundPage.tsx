import { useNavigate } from 'react-router-dom'

import { Box, Button, Typography } from '@mui/material'

import { paths } from '@/paths.ts'

const NotFoundPage = () => {
    const navigate = useNavigate()

    return (
        <div>
            <Typography variant="h1">
                <span role="img" aria-hidden>
                    😕
                </span>{' '}
                404 - Page not found{' '}
                <span role="img" aria-hidden>
                    😕
                </span>
            </Typography>
            <Typography variant="body1">
                We are sorry but page does not exist, why not try one that does?
            </Typography>
            <Box
                sx={{
                    my: 3,
                    display: 'flex',
                    flexDirection: { xs: 'column', md: 'row' },
                    gap: 3,
                    alignItems: 'center',
                }}
            >
                <Button
                    variant="outlined"
                    onClick={() => navigate(`${paths.movieDetail}508883`)}
                >
                    Show me your favourite movie
                </Button>
                <Typography variant="body2" fontWeight={700}>
                    or
                </Typography>
                <Button
                    variant="outlined"
                    onClick={() => navigate(paths.homepage)}
                >
                    Take me to search
                </Button>
            </Box>
        </div>
    )
}

export default NotFoundPage
