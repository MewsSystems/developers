import { FC } from 'react';
import LoadingBar from '../../../shared/components/LoadingBar/LoadingBar';
import Pagination from '../../../shared/components/Pagination/Pagination';
import MovieSearchInput from '../MovieSearchInput/MovieSearchInput';
import StyledMovieSearchHeader, {
    StyledSearchContainer,
} from './MovieSearchHeader.styles';

interface Props {
    searchQuery: string;
    currentPageNumber: number;
    totalPages: number;
    showPagination: boolean;
    isLoading: boolean;
    onSearchChange: (value: string) => void;
    onPageChange: (page: number) => void;
}

const MovieSearchHeader: FC<Props> = ({
    searchQuery,
    currentPageNumber,
    totalPages,
    showPagination,
    isLoading,
    onSearchChange,
    onPageChange,
}) => {
    return (
        <StyledMovieSearchHeader>
            <StyledSearchContainer>
                <MovieSearchInput
                    value={searchQuery}
                    onChange={onSearchChange}
                />
                {showPagination && (
                    <Pagination
                        page={currentPageNumber}
                        totalPages={totalPages}
                        onChange={onPageChange}
                    />
                )}
            </StyledSearchContainer>
            <LoadingBar loading={isLoading} />
        </StyledMovieSearchHeader>
    );
};

export default MovieSearchHeader;
