import styled from 'styled-components';
import { Color } from '../../enums/style/color.ts';

export const FallbackImgImg = styled.img`
    width: 100%;
`;

export const FallbackImgPlaceholder = styled.div`
    color: ${Color.Accent};
    background-color: ${Color.SecondaryAccent};
    width: 100%;
    display: flex;
    flex-wrap: nowrap;
    justify-content: center;
    align-items: center;
`;