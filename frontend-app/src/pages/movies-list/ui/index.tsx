import { useState, useCallback } from 'react';
import { debounce } from '@tanstack/react-pacer/debouncer'
import { Text, HStack, VStack, Card } from "@chakra-ui/react"
import useQueryMovies from '@/entities/movie/hooks/useQueryMovies';
import useQueryConfiguration from '@/entities/configuration/hooks/useQueryConfiguration';

import { toLocaleDate } from '@/shared/lib/utils';
import { usePreferredLanguage } from '@uidotdev/usehooks';
import Pagination from '@/shared/ui/Pagination';
import type { MovieListItem } from '@/entities/movie/types';
import type { ConfigurationImages } from '@/entities/configuration/types';

export function Index() {
    const [page, setPage] = useState("1");
    const [searchText, setSearchText] = useState('')
    const [debouncedSearchText, setDebouncedSearchText] = useState('')

    const debouncedSetSearch = useCallback(
        debounce(setDebouncedSearchText, {
            wait: 800,
        }),
        [],
    )

    function handleSearchChange(e: React.ChangeEvent<HTMLInputElement>) {
        const newValue = e.target.value
        setSearchText(newValue)
        debouncedSetSearch(newValue)
    }

    const { data, isLoading } = useQueryMovies({ query: debouncedSearchText, page })
    const { data: dataConfiguration, isLoading: isLoadingConfiguration } = useQueryConfiguration();
    return (
        <div className="p-2" style={{ paddingBottom: "40px" }}>
            <input
                autoFocus
                type="search"
                value={searchText}
                onChange={handleSearchChange}
                placeholder="Type to search..."
                style={{ width: '100%' }}
            />
            {isLoading && isLoadingConfiguration ? 'loading' : <div>
                {data && dataConfiguration && <MoviesCards movies={data.results} boundIncludeConfiguration={includeConfiguration.bind(null, dataConfiguration.images)} />}
            </div>}
            {data && data.total_results > 20 && <Pagination page={page} pageSize={data?.total_pages} onPageChange={(pageChangeDetails: any) => {
                setPage(pageChangeDetails.page)
            }} />}
        </div>
    )
}

function MoviesCards({ movies, boundIncludeConfiguration }: { movies: MovieListItem[], boundIncludeConfiguration: (path: string) => string }) {
    return movies.map(movie => <MovieCard key={movie.id} movie={movie} boundIncludeConfiguration={boundIncludeConfiguration} />)
}

function MovieCard({ movie, boundIncludeConfiguration }: { movie: MovieListItem, boundIncludeConfiguration: (path: string) => string }) {
    const language = usePreferredLanguage();
    return <Card.Root width="720px" variant={"subtle"} key={movie.id}>
        <Card.Body gap="2">
            <HStack>
                    <img src={movie.poster_path ? boundIncludeConfiguration(movie.poster_path) : ""}/>
                <VStack>
                    <Card.Title mb="2">
                        <Text> <a href={`/moviedetails/${movie.id}`}>{movie.title}</a></Text>
                        <Text>
                            {toLocaleDate(movie.release_date, language)}
                        </Text>
                    </Card.Title>
                    <Card.Description>
                        {movie.overview}
                    </Card.Description>
                </VStack>
            </HStack>
        </Card.Body>
    </Card.Root>

}

function includeConfiguration(configurationImages: ConfigurationImages, path: string) {
    return configurationImages.base_url + configurationImages.poster_sizes[1] + path;
}
