import * as React from 'react';
import { CircleLoader } from 'react-spinners';
import { css } from '@emotion/core';

const override = css`
    display: block;
    margin: 0 auto;
`;

export const Spinner = () => <CircleLoader css={override as any} color="#1245A8" />