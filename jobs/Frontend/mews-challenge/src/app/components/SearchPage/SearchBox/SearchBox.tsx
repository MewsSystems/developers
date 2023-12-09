import React, { FC, useEffect } from 'react';
import Input from '../../../elements/Input';
import { useDispatch, useSelector } from 'react-redux'
import Spinner from '../../common/Spinner/Spinner';
import Show from '../../../components/utils/Show';
import ResultList from '../ResultList/ResultList';
import { selectPage, selectQuery, setPage, setQuery, setTotalPages } from '../../../services/appReducer';
import { useDebounce } from '../../../hooks';
import { AppDispatch } from '../../../store';
import PageSelector from '../PageSelector/PageSelector';
import { useGetMoviesQuery } from '../../../services/movie';
import Box from '../../../elements/Box';
import { Spacing } from '../../../types';


const SearchBox: FC = () => {
    const dispatch = useDispatch<AppDispatch>();
    const debounce = useDebounce(500);

    const query = useSelector(selectQuery);
    const page = useSelector(selectPage);
    const { data: response, error, isFetching } = useGetMoviesQuery({ query, page });

    const { results: movies = [], total_pages: totalPages = 1 } = response || {};

    useEffect(() => {
        dispatch(setTotalPages(totalPages));
    }, [response]);

    const onChangeQuery = (e: { target: { value: string } }) => {
        dispatch(setQuery(e.target.value));
        dispatch(setPage(1));
    }

    return (
        <>
            <Input placeholder='Search!' onChange={debounce(onChangeQuery)} />
            <Box pt={Spacing.base}>
                <Show when={isFetching}>
                    <Spinner />
                </Show>
                <Show when={!isFetching}>
                    <Show when={query.length > 0}>
                        <ResultList movies={movies} />
                    </Show>
                    <Show when={movies.length > 0}>
                        <PageSelector />
                    </Show>
                </Show>
            </Box>
        </>
    )
}

export default SearchBox;