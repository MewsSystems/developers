import styled from 'styled-components';
import { Spacer } from '../enums/style/spacer.ts';
import { Link } from 'react-router-dom';
import { Color } from '../enums/style/color.ts';
import { FontSize } from '../enums/style/fontSize.ts';
import { BorderRadius } from '../enums/style/borderRadius.ts';
import { BsFilm } from 'react-icons/bs';
import { Breakpoint } from '../enums/style/breakpoint.ts';

export const MoviesListLi = styled.li`
    display: block;
    width: 100%;
    border-radius: ${BorderRadius.Md};
    overflow: hidden;
    margin-bottom: ${Spacer.Md};
    transition: box-shadow .3s;

    @media (min-width: ${Breakpoint.Sm}) {
        width: calc(50% - ${Spacer.Md});
        margin-right: ${Spacer.Md};
    }
    
    @media (min-width: ${Breakpoint.Md}) {
        width: calc(25% - ${Spacer.Md});
    }

    &:hover,
    &:focus {
        box-shadow: 0px 3px 5px -1px rgba(0, 0, 0, 0.2),
        0px 6px 10px 0px rgba(0, 0, 0, 0.14),
        0px 1px 18px 0px rgba(0, 0, 0, 0.12);
    }
`;

export const MovieCard = styled(Link)`
    overflow: hidden;
    border-top-left-radius: ${BorderRadius.Md};
    border-top-right-radius: ${BorderRadius.Md};
    border: 2px solid ${Color.Accent};
    display: block;
    text-decoration: none;
    border-image:
            linear-gradient(
                    to bottom,
                    rgba(0, 0, 0, 0),
                    ${Color.Accent}
            ) 1 100%;
    
`;

export const MovieCardBody = styled.div`
    padding: ${Spacer.Md};
`;

export const MovieCardTitle = styled.h5`
    line-height: 1.2;
    font-size: ${FontSize.Md};
    height: ${parseFloat(FontSize.Md) * 1.2 * 3}rem;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
    overflow: hidden;
`;

export const MovieCardOverview = styled.p`
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
`;

// TODO: add fallback image
export const MovieCardImg = styled.img`
    width: 100%;
    aspect-ratio: 2 / 3;
`;

export const MovieCardImgPlaceholder = styled.div`
    color: ${Color.Primary};
    background-color: ${Color.SecondaryAccent};
    width: 100%;
    aspect-ratio: 2 / 3;
    display: flex;
    flex-wrap: nowrap;
    justify-content: center;
    align-items: center;
`;

export const MovieCardImgPlaceholderIcon = styled(BsFilm)`
    font-size: 6rem;
    opacity: .75;
    transform: rotate(30deg);
`;