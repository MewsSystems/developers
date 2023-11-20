import React from 'react';
import { Color, FontSize, Spacing } from '../../types';
import Box from '../../elements/Box';
import Heading from '../../elements/Heading';
import SearchBox from './SearchBox/SearchBox';
import { BiSolidCameraMovie } from "react-icons/bi";

function SearchPage() {
    return (
        <Box>
            <Heading textAlign='center' color={Color['secondary.light']} size={FontSize.huge}>
                <BiSolidCameraMovie/> MewMovier
            </Heading>
            <Box>
                <SearchBox />
            </Box>
        </Box>
    );
}

export default SearchPage;
