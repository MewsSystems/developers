import React from 'react';
import { useDispatch } from 'react-redux';
import styled from 'styled-components';
import { changePage } from './store/search-movie-actions';
import { useAppSelector } from '../../store';

const Ul = styled.ul`
    padding: 0;
`;

const Li = styled.li`
    display: inline-block;
    list-style: none;
`;

const A = styled.a`
    display: inline-block;
    padding: 8px;
    margin: 0 2px;
    background: ${(props: { active: boolean }) => (props.active ? 'red' : 'black')};
    color: white;
    text-decoration: none;
    transition: 300ms background;
    &:hover {
        background: red;
    }
`;


export default function Pagination() {
    const dispatch = useDispatch();
    const { totalPages, query, page: currentPage } = useAppSelector((state) => state.search);

    const getPage = (page: number, { after, before }: { after: string, before: string }) => {
        return (
            <Li
                key={`${query}${page}`}
            >
                {before}
                <A
                    href={''}
                    onClick={(e) => {
                        e.preventDefault();
                        dispatch(changePage(page));
                    }}
                    active={currentPage === page}
                >
                    {page}
                </A>
                {after}
            </Li>
        );
    };

    const pages = [...new Array(totalPages).keys()].map((item, key) => {
        return key + 1;
    });

    const relevantPages = pages.filter((page) => {
        if (page === 1) {
            return true;
        }

        if (Math.abs(page - currentPage) <= 5) {
            return true;
        }

        if (page === totalPages) {
            return true;
        }

        return false;
    });

    return (
        <Ul>
            {
                relevantPages.map((page, index: number) => {
                    return getPage(page, {
                        after: page === 1 && relevantPages[index + 1] !== 2 ? '...' : '',
                        before: page === totalPages && relevantPages[index - 1] !== totalPages - 1 ? '...' : '',
                    });
                })
            }
        </Ul>
    );
}
