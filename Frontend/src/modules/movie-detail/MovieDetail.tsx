import React from 'react';
import styled from 'styled-components';
import { MovieDetail as MovieDetailType } from '../../types';

const Wrap = styled.div`
    display: flex;
    flex-direction: row;
    @media (max-width: 400px) {
        flex-direction: column;
    }
`;

const Feature = styled.div`
    margin-right: 20px;
    img {
        max-width: 200px;
        @media (max-width: 400px) {
            max-width: 100%;
        }
    }
`;

const Field = styled.div`
    padding-bottom: 20px;
    strong {
        display: block
    }
`;

export default function MovieDetail(props: MovieDetailType) {
    const getField = (name: string, content: string) => {
        return (
            <Field>
                <strong>
                    {`${name}:`}
                </strong>
                {content}
            </Field>
        );
    };

    return (
        <Wrap>
            {props.poster_path ? (<Feature><img src={props.poster_path} alt={props.title} /></Feature>) : null}
            <div>
                {getField('Title', props.title)}
                {getField('Overview', props.overview)}
            </div>
        </Wrap>
    );
}
