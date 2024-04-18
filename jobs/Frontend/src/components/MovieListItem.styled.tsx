import styled from 'styled-components';
import { Spacer } from '../enums/style/spacer.ts';
import { Link } from 'react-router-dom';
import { Color } from '../enums/style/color.ts';
import { FontSize } from '../enums/style/fontSize.ts';
import { BorderRadius } from '../enums/style/borderRadius.ts';
import { BsFilm } from 'react-icons/bs';
import { Breakpoint } from '../enums/style/breakpoint.ts';
import { TransitionDuration } from '../enums/style/transitionDuration.ts';

export const MoviesListLi = styled.li`
    display: block;
    width: 100%;
    border-top-left-radius: ${BorderRadius.Md};
    border-top-right-radius: ${BorderRadius.Md};
    border-bottom: 2px solid ${Color.Accent};
    overflow: hidden;
    margin-bottom: ${Spacer.Md};
    transition: box-shadow ${TransitionDuration.Medium};

    @media (min-width: ${Breakpoint.Sm}) {
        width: calc(50% - ${Spacer.Md});
        margin-right: ${Spacer.Md};
    }
    
    @media (min-width: ${Breakpoint.Md}) {
        width: calc(25% - ${Spacer.Md});
    }

    &:hover,
    &:focus {
        box-shadow: 0px -1px 8px 2px rgba(0,0,0,0.3);
    }
`;

export const MovieCard = styled(Link)`
    overflow: hidden;
    height: 100%;
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
    transition: background ${TransitionDuration.Medium};

    &:hover,
    &:focus {
        background: linear-gradient(180deg, rgba(173,0,10,0) 75%, rgba(255,209,0,1) 100%);
    }
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

export const MovieCardImg = styled.img`
    width: 100%;
    aspect-ratio: 2 / 3;
`;

export const MovieCardImgPlaceholder = styled.div`
    color: ${Color.SecondaryAccent};
    background-color: ${Color.Accent};
    width: 100%;
    aspect-ratio: 2 / 3;
    display: flex;
    flex-wrap: nowrap;
    justify-content: center;
    align-items: center;
`;

export const MovieCardImgPlaceholderIcon = styled(BsFilm)`
    font-size: 6rem;
    transform: rotate(30deg);
`;