import { Search } from '@mui/icons-material'
import {
    Box,
    Divider,
    InputAdornment,
    OutlinedInput,
    Stack,
    Typography,
} from '@mui/material'
import { HomePageViewProps } from './types'
import { FC } from 'react'

export const HomePageView: FC<HomePageViewProps> = ({
    submitSearchedTitle,
}) => {
    return (
        <Box component='main'>
            <Stack
                component='section'
                className='container mt-8 max-w-[48rem] gap-6 md:mt-10 md:gap-12 lg:mt-28 lg:gap-20'
            >
                <Stack className='w-full items-center gap-4 text-center'>
                    <Typography
                        variant='h1'
                        className='text-secondary-main text-6xl font-medium md:text-7xl'
                    >
                        Flick Index
                    </Typography>
                    <Divider className='w-11' />
                    <Typography
                        variant='caption'
                        className='text-xl font-extralight text-gray-700'
                    >
                        Search. Discover. Enjoy
                    </Typography>
                </Stack>
                <OutlinedInput
                    onChange={({ target }) => submitSearchedTitle(target.value)}
                    startAdornment={
                        <InputAdornment position='start'>
                            <Search />
                        </InputAdornment>
                    }
                    placeholder='Batman: begins...'
                />
            </Stack>
        </Box>
    )
}
