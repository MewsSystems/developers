import React from 'react';
import styled from 'styled-components';
import { useDispatch } from 'react-redux';
import { changeQuery } from './store/search-movie-actions';
import { useAppSelector } from '../../store';

const Input = styled.input`
  padding: 10px;
  display: block;
  width: 100%;
`;

interface Props {
    defaultQuery: string,
}

export default function SearchInput({ defaultQuery }: Props) {
    const dispatch = useDispatch();
    const { query } = useAppSelector((state) => state.search);

    let valueSetTimeout: number;

    React.useEffect(() => {
        if (query !== defaultQuery) {
            dispatch(changeQuery(defaultQuery));
        }
        return () => {
            clearTimeout(valueSetTimeout);
        };
    }, []);

    const onInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const { target } = event;
        clearTimeout(valueSetTimeout);
        valueSetTimeout = setTimeout(() => {
            dispatch(changeQuery(target.value));
        }, 500);
    };

    return (
        <Input
            onChange={onInputChange}
            defaultValue={defaultQuery}
            placeholder={'Type a movie title'}
        />
    );
}
