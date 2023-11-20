import React, { FC } from 'react';
import Button from '../../../elements/Button';
import Text from '../../../elements/Text';
import { useDispatch, useSelector } from 'react-redux';
import { selectPage, selectTotalPages, setPage } from '../../../services/appReducer';
import Show from '../../utils/Show';
import { AppDispatch } from '../../../store';
import Box from '../../../elements/Box';
import { Color, Spacing } from '../../../types';

const PageSelector: FC = () => {
    const dispatch = useDispatch<AppDispatch>();

    const page = useSelector(selectPage);
    const totalPages = useSelector(selectTotalPages);

    const changePage = (page: number) => {
        dispatch(setPage(page));
    }

    return (
        <Box textAlign='center'>
            <Show when={page > 1}>
                <Button onClick={() => changePage(page-1)}>&lt;</Button>
            </Show>
            <Box inline p={Spacing.big}>
                <Text color={Color['primary.dark']}>{page}</Text>
            </Box>
            <Show when={page < totalPages}>
                <Button onClick={() => changePage(page+1)}>&gt;</Button>
            </Show>
        </Box>
    )
}

export default PageSelector;