import {useNavigate, useSearchParams} from "react-router-dom"
import styled from "styled-components"
import {useMovies} from "../api/useMovies.ts";
import {getPosterSrc} from "../utils/getPosterSrc.ts";
import {useDebouncedValue} from "../utils/useDebouncedValue.tsx";
import {colors, fontSizes, layout, radii, shadows, spacing} from "../styles/designTokens.ts";

export const SearchView = () => {
    const navigate = useNavigate();
    const [searchParams, setSearchParams] = useSearchParams("");
    const query = searchParams.get("query") || ""
    const debouncedQuery = useDebouncedValue(query)

    const {data, fetchNextPage, hasNextPage, isError, error, isFetching} = useMovies(debouncedQuery);

    const movies = data?.pages.flatMap((page) => page?.results) || [];
    const isQuery = debouncedQuery !== ""
    const noResults = isQuery && !isFetching && movies.length === 0;

    return (
        <Container>
            <SearchInput
                type="text"
                placeholder="Search movies..."
                value={query}
                onChange={(e) => {
                    const value = e.target.value
                    setSearchParams({query: value})
                }}
            />
            {isError && <ErrorMessage>❌ Error: {error.message}</ErrorMessage>}
            {noResults && <NoResultsText>😞 No movies found for "{query}".</NoResultsText>}
            <MovieList>
                {movies.map((movie) => (
                    <MovieItem key={movie.id} onClick={() => navigate(`/movie/${movie.id}`)}>
                        <picture>
                            <source srcSet={getPosterSrc(movie.poster_path, "webp")} type="image/webp" />
                            <source srcSet={getPosterSrc(movie.poster_path, "jpg")} type="image/jpeg" />
                            <MoviePoster src={getPosterSrc(movie.poster_path)} alt={movie.title || "Placeholder Poster"} />
                        </picture>

                        <MovieTitleWrapper>
                            <MovieTitle>{movie.title}</MovieTitle>
                            {movie.release_date !== '' &&
                                <MovieYear>({movie.release_date?.split("-")[0]})</MovieYear>
                            }
                        </MovieTitleWrapper>
                    </MovieItem>
                ))}
            </MovieList>
            {hasNextPage &&
                <ButtonWrapper><LoadMoreButton onClick={() => fetchNextPage()}>Load
                    more</LoadMoreButton></ButtonWrapper>}
        </Container>
    )
}

const Container = styled.div`
    margin: 0 auto;
    padding: ${spacing.xl};

    max-width: ${layout.containerWidth};
`

const SearchInput = styled.input`
    padding: ${spacing.sm} ${spacing.md};
    width: 100%;

    color: ${colors.text};
    font-size: ${fontSizes.base};

    border: 2px solid ${colors.border};
    border-radius: ${radii.md};
    background: ${colors.background};
    outline: none;
    box-shadow: ${shadows.input};
    transition: border 0.2s ease, box-shadow 0.2s ease;

    margin: 0 auto ${spacing.xl};

    &:focus {
        border-color: ${colors.primary};
        box-shadow: ${shadows.focus};
    }

    &::placeholder {
        color:  ${colors.textLight};
        font-style: italic;
    }
`

const ErrorMessage = styled.p`
    color: ${colors.error};
    font-weight: 600;
    font-size: ${fontSizes.sm};
    text-align: center;
    margin-top: ${spacing.sm};
`

const NoResultsText = styled.p`
    font-size: ${fontSizes.lg};
    color: ${colors.textMuted};
    text-align: center;
    margin: ${spacing.lg} 0;
`

const MovieList = styled.div`
    margin-bottom: ${spacing.lg};

    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 240px));
    gap: ${spacing.lg};
    justify-content: center;
    
    @media (max-width: 768px) {  
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    }
`

const MovieItem = styled.div`
    padding: ${spacing.sm};

    display: flex;
    flex-direction: column;
    align-items: center;

    border: 1px solid ${colors.borderDark};
    border-radius: ${radii.md};
    background: ${colors.background};
    cursor: pointer;
    transition: background 0.2s ease;

    &:hover {
        background: ${colors.backgroundAlt};
    }
`

const MoviePoster = styled.img`
    width: 100%;
    height: 300px;
    object-fit: cover;
    border-radius: ${radii.sm};
`

const MovieTitleWrapper = styled.div`
    margin-top: ${spacing.sm};
    
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
`

const MovieTitle = styled.p`
    font-size: ${fontSizes.base};
    font-weight: 600;

    text-align: center;
`

const MovieYear = styled.span`
    font-size: ${fontSizes.sm};
    color: ${colors.textLight};
`

const ButtonWrapper = styled.div`
    margin-top: ${spacing.xl};
    
    display: flex;
    justify-content: center;
`

const LoadMoreButton = styled.button`
    padding: ${spacing.sm} ${spacing['2xl']};

    color: ${colors.background};
    font-weight: 600;

    border-radius: ${radii.sm};
    background: ${colors.primary};

    cursor: pointer;
    transition: box-shadow 0.2s ease;

    &:hover {
        box-shadow: ${shadows.hover};
    }

    &:focus {
        outline: none;
        box-shadow: ${shadows.focus};
    }
`