import React, { useState } from 'react';
import { Text } from '../../../elements/Text';
import { Color, FontSize, Spacing } from '../../../types';
import Box from '../../../elements/Box';
import Show from '../../utils/Show';

function Rating({ count, rating, starCount = 10 }: any) {

    const stars = [];
    for (let i = 0; i < starCount; i++) {
        stars.push(i < starCount*rating/10);
    }

    return (
        <Box mb={Spacing.base}>
            {stars.map((e, i) => <Text key={i} size={FontSize.small} color={Color[e ? 'secondary.light' : 'secondary.dark']}>{e ? '★' : '☆'}</Text>)}
            <Show when={count !== undefined}>
                <Box inline ml={Spacing.base}>
                    <Text size={FontSize.small}>({count})</Text>
                </Box>
            </Show>
        </Box>
    );
}

export default Rating;
