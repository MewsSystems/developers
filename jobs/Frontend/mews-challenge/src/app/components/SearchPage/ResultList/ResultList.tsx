import React, { FC } from 'react';
import { Movie, Spacing } from '../../../types';
import Text from '../../../elements/Text';
import Box from '../../../elements/Box';
import Show from '../../utils/Show';
import ResultLink from '../ResultLink/ResultLink';


const ResultList: FC<{ movies: Movie[] }> = ({ movies }) => {
    return (
        <Box my={Spacing.small} style={{ maxHeight: '70vh', overflow: 'auto' }}>
            <Show when={movies.length > 0}>
                {
                    movies.map((movie: Movie, i) => <ResultLink key={i} movie={movie} />)
                }
            </Show>
            <Show when={movies.length === 0}>
                <Text>No results</Text>
            </Show>
        </Box>
    )
}

export default ResultList;