import React, { FC } from 'react';
import Box from '../../../elements/Box';
import { RiMovie2Line } from 'react-icons/ri';
import { Color, FontSize, Spacing } from '../../../types';

const spinnerAnimation = {
    animation: 'spin 1s linear infinite',
}

const Spinner: FC = () => (
    <Box textAlign='center' p={Spacing.huge} fontSize={FontSize.big}>
        <RiMovie2Line style={{...spinnerAnimation}} color={Color['secondary.light']}/>
    </Box>
)

export default Spinner;