import { CalendarToday, Language, AccessTime } from '@mui/icons-material'
import { Box, Chip, Rating, Skeleton, Stack, Typography } from '@mui/material'
import type { TopSummaryProps } from '../types'

export const TopSummary = ({ detailData, isLoading }: TopSummaryProps) => {
    const stopedLoadingWithData = !isLoading && detailData

    return (
        <>
            <Stack className='gap-4 rounded-md bg-white px-4 py-3 sm:flex-row md:gap-6 md:px-6 md:py-5'>
                <Stack className='gap-2'>
                    {stopedLoadingWithData ? (
                        <>
                            <Typography className='text-[2rem] font-medium leading-none sm:text-[3.2rem] md:text-8xl md:leading-[4rem]'>
                                {Math.round(detailData?.vote_average * 10)}%
                            </Typography>
                            <Stack>
                                <Rating
                                    value={detailData?.vote_average / 2}
                                    precision={0.25}
                                    readOnly
                                    size='small'
                                />
                                <Typography>
                                    total: {detailData?.vote_count}
                                </Typography>
                            </Stack>
                        </>
                    ) : (
                        <>
                            <Skeleton
                                data-testid='Skeleton'
                                className='h-24 w-44 scale-100'
                            />
                            <Stack>
                                <Skeleton
                                    data-testid='Skeleton'
                                    className='scale-80 h-9 w-44'
                                />
                                <Skeleton
                                    data-testid='Skeleton'
                                    className='scale-80 h-9 w-40'
                                />
                            </Stack>
                        </>
                    )}
                </Stack>
                <Stack className='gap-6'>
                    <Stack className='flex-wrap gap-1 sm:flex-row sm:gap-3'>
                        <Stack className='flex-row items-center gap-1'>
                            <CalendarToday fontSize='small' />
                            {stopedLoadingWithData ? (
                                <Typography>
                                    {detailData?.release_date.substring(0, 4)}
                                </Typography>
                            ) : (
                                <Skeleton
                                    data-testid='Skeleton'
                                    className='scale-80 h-9 w-10'
                                />
                            )}
                        </Stack>
                        <Stack className='flex-row items-center gap-1'>
                            <Language fontSize='small' />
                            {stopedLoadingWithData ? (
                                <Typography>
                                    {detailData?.original_language}
                                </Typography>
                            ) : (
                                <Skeleton
                                    data-testid='Skeleton'
                                    className='scale-80 h-9 w-10'
                                />
                            )}
                        </Stack>
                        <Stack className='flex-row items-center gap-1'>
                            <AccessTime fontSize='small' />
                            {stopedLoadingWithData ? (
                                <Typography>
                                    {detailData?.runtime}min
                                </Typography>
                            ) : (
                                <Skeleton
                                    data-testid='Skeleton'
                                    className='scale-80 h-9 w-10'
                                />
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
                                      test-id={`genre-chip-${genre.id}`}
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
                <Stack className='mb-2 flex-row flex-wrap gap-2 sm:hidden'>
                    {stopedLoadingWithData
                        ? detailData?.genres.map((genre) => (
                              <Chip
                                  key={genre.id}
                                  label={genre.name}
                                  size='small'
                                  className='rounded-lg text-sm'
                              />
                          ))
                        : [...Array(3)].map((_, index) => (
                              <Skeleton
                                  key={index}
                                  className='h-9 w-16'
                              />
                          ))}
                </Stack>
                <Typography
                    variant='h1'
                    className=' hidden pb-2 text-2xl font-medium sm:inline-block md:text-4xl'
                >
                    {detailData?.title}
                </Typography>
                <Typography className='hidden text-base sm:inline-block '>
                    {detailData?.overview}
                </Typography>
            </Box>
        </>
    )
}
