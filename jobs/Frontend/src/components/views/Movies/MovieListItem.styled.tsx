import styled from 'styled-components';
import { Spacer } from '../../../enums/style/spacer';
import { Link } from 'react-router-dom';
import { Color } from '../../../enums/style/color';
import { FontSize } from '../../../enums/style/fontSize';
import { BorderRadius } from '../../../enums/style/borderRadius';
import { BsFilm } from 'react-icons/bs';
import { Breakpoint } from '../../../enums/style/breakpoint';
import { TransitionDuration } from '../../../enums/style/transitionDuration';

const cardBorderWidth = '2px';

export const MoviesListLi = styled.li`
    display: block;
    width: 100%;
    border-top-left-radius: ${BorderRadius.Md};
    border-top-right-radius: ${BorderRadius.Md};
    border-bottom: ${cardBorderWidth} solid ${Color.Accent};
    margin-bottom: ${Spacer.Md};

    @media (min-width: ${Breakpoint.Xs}) {
        width: calc(50% - ${Spacer.Sm});

        &:nth-child(even) {
            margin-left: ${Spacer.Sm};
        }

        &:nth-child(odd) {
            margin-right: ${Spacer.Sm};
        }
    }

    @media (min-width: ${Breakpoint.Md}) {
        // 1/4 of the width - average spacing around the items
        width: calc(25% - ${Spacer.Sm} - ${Spacer.Xs});

        &:nth-child(4n + 2),
        &:nth-child(4n + 3) {
            margin-left: ${Spacer.Sm};
            margin-right: ${Spacer.Sm};
        }
    }
`;

export const MovieCard = styled(Link)`
    display: flex;
    flex-wrap: wrap;
    overflow: hidden;
    height: 100%;
    border-top-left-radius: ${BorderRadius.Md};
    border-top-right-radius: ${BorderRadius.Md};
    border: ${cardBorderWidth} solid ${Color.Accent};
    border-bottom: none;
    border-top: ${cardBorderWidth} solid ${Color.Background};
    text-decoration: none;
    border-image: linear-gradient(
            to bottom,
            rgba(0, 0, 0, 0),
            ${Color.Accent}
    ) 1 100%;
    transition: box-shadow ${TransitionDuration.Medium};

    &:hover,
    &:focus {
        box-shadow: 0px -1px 8px 2px rgba(0, 0, 0, 0.3);

        .movie-card-body {
            background: linear-gradient(0deg, ${Color.SecondaryAccent} 0%, transparent 100%);
        }
    }
`;

export const MovieCardBody = styled.div`
    padding: ${Spacer.Md};
    width: 100%;
    transition: background ${TransitionDuration.Medium};
`;

export const MovieCardTitle = styled.h5`
    line-height: 1.2;
    font-size: ${FontSize.Md};
    height: ${parseFloat(FontSize.Md) * 1.2 * 3}rem;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
    overflow: hidden;
    margin-top: 0;
`;

export const MovieCardImgPlaceholderIcon = styled(BsFilm)`
    font-size: 6rem;
    transform: rotate(30deg);
`;