import { Home } from '@mui/icons-material'
import { Button, Stack, Typography } from '@mui/material'
import { useNavigate } from 'react-router-dom'

export const FallbackPage = () => {
    const navigate = useNavigate()
    return (
        <Stack className='container items-center pt-11 sm:pt-44 md:pt-60'>
            <Typography
                variant='h1'
                className='text-6xl sm:text-[9rem] md:text-[18rem]'
            >
                404
            </Typography>
            <Typography className='mb-8 max-w-96'>
                🕵️‍♂️ It's a mystery too complex even for us! The page you wanted
                isn't here, and we need your detective skills to find the right
                script.{' '}
                <span className='font-bold'>
                    Why not start over at our homepage?
                </span>
            </Typography>
            <Button
                variant='contained'
                endIcon={<Home />}
                className='normal-case'
                onClick={() => navigate('/')}
            >
                Return to HQ
            </Button>
        </Stack>
    )
}
