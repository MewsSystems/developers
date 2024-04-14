import { CalendarToday, Language, AccessTime } from '@mui/icons-material'
import { Box, Chip, Rating, Skeleton, Stack, Typography } from '@mui/material'
import type { TopSummaryProps } from '../types'

export const TopSummary = ({ detailData, isLoading }: TopSummaryProps) => {
    const stopedLoadingWithData = !isLoading && detailData

    return (
        <>
            <Stack className='flex-row gap-6 rounded-md bg-white px-4 py-3 md:px-6 md:py-5'>
                <Stack className='gap-2'>
                    {stopedLoadingWithData ? (
                        <>
                            <Typography className='text-8xl font-medium leading-[4rem]'>
                                {Math.round(detailData?.vote_average * 10)}%
                            </Typography>
                            <Stack>
                                <Rating
                                    value={detailData?.vote_average / 2}
                                    precision={0.25}
                                    readOnly
                                />
                                <Typography>
                                    total: {detailData?.vote_count}
                                </Typography>
                            </Stack>
                        </>
                    ) : (
                        <>
                            <Skeleton className='h-24 w-44 scale-100' />
                            <Stack>
                                <Skeleton className='scale-80 h-9 w-44' />
                                <Skeleton className='scale-80 h-9 w-40' />
                            </Stack>
                        </>
                    )}
                </Stack>
                <Stack className='gap-6'>
                    <Stack className='flex-wrap gap-3 sm:flex-row'>
                        <Stack className='flex-row items-center gap-1'>
                            <CalendarToday fontSize='small' />
                            {stopedLoadingWithData ? (
                                <Typography>
                                    {detailData?.release_date.substring(0, 4)}
                                </Typography>
                            ) : (
                                <Skeleton className='scale-80 h-9 w-10' />
                            )}
                        </Stack>
                        <Stack className='flex-row items-center gap-1'>
                            <Language fontSize='small' />
                            {stopedLoadingWithData ? (
                                <Typography>
                                    {detailData?.original_language}
                                </Typography>
                            ) : (
                                <Skeleton className='scale-80 h-9 w-10' />
                            )}
                        </Stack>
                        <Stack className='flex-row items-center gap-1'>
                            <AccessTime fontSize='small' />
                            {stopedLoadingWithData ? (
                                <Typography>
                                    {detailData?.runtime}min
                                </Typography>
                            ) : (
                                <Skeleton className='scale-80 h-9 w-10' />
                            )}
                        </Stack>
                    </Stack>
                    <Stack className='hidden flex-row flex-wrap gap-2 sm:flex'>
                        {stopedLoadingWithData
                            ? detailData?.genres.map((genre) => (
                                  <Chip
                                      key={genre.id}
                                      label={genre.name}
                                      className='rounded-lg text-base'
                                  />
                              ))
                            : [...Array(3)].map((_, index) => (
                                  <Skeleton
                                      key={index}
                                      className='h-9 w-16'
                                  />
                              ))}
                    </Stack>
                </Stack>
            </Stack>
            <Box>
                <Typography
                    variant='h1'
                    className='pb-2 text-4xl font-medium'
                >
                    {detailData?.title}
                </Typography>
                <Typography className='text-base'>
                    {detailData?.overview}
                </Typography>
            </Box>
        </>
    )
}
