import { Clear, Search } from '@mui/icons-material'
import {
    Box,
    Divider,
    FormControl,
    FormHelperText,
    IconButton,
    InputAdornment,
    OutlinedInput,
    Stack,
    Typography,
} from '@mui/material'
import type { HomePageViewProps } from './types'
import type { FC } from 'react'
import { HomeSearchContent } from './components'

export const HomePageView: FC<HomePageViewProps> = ({ movieSearch }) => {
    const {
        submitSearchedTitle,
        currentSearchTitle,
        clearTitle,
        searchInputRef,
        ...searchContentData
    } = movieSearch

    const showHelper =
        currentSearchTitle.length > 0 && currentSearchTitle.length < 3

    return (
        <Box
            component='main'
            className='bg-gradient-to-b from-primary-main to-white bg-[length:100%_18rem] bg-no-repeat pb-7'
        >
            <Stack
                component='section'
                className='container max-w-[48rem] gap-6 pb-9 pt-8 md:gap-12 md:pt-10 lg:gap-20 lg:pt-28'
            >
                <Stack className='w-full items-center gap-4 text-center'>
                    <Typography
                        variant='h1'
                        className='text-6xl font-medium text-secondary-main md:text-7xl'
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
                <FormControl>
                    <OutlinedInput
                        className='bg-white'
                        inputRef={searchInputRef}
                        onChange={({ target }) =>
                            submitSearchedTitle(target.value)
                        }
                        startAdornment={
                            <InputAdornment position='start'>
                                <Search />
                            </InputAdornment>
                        }
                        endAdornment={
                            currentSearchTitle.length > 0 && (
                                <InputAdornment position='end'>
                                    <IconButton
                                        size='small'
                                        onClick={clearTitle}
                                        data-testid='clear-search'
                                    >
                                        <Clear />
                                    </IconButton>
                                </InputAdornment>
                            )
                        }
                        placeholder='Batman: begins...'
                    />
                    {showHelper && (
                        <FormHelperText id='search-helper-text'>
                            You need to type at least 3 characters to search
                        </FormHelperText>
                    )}
                </FormControl>
            </Stack>
            {currentSearchTitle.length > 2 ? (
                <HomeSearchContent {...searchContentData} />
            ) : null}
        </Box>
    )
}
