import React from 'react';
import { NotFoundPageWrapper } from './not-found.styled';

export const NotFound = () => {
    return (
        <NotFoundPageWrapper>
            <h1>Page Not Found</h1>
            <p>Sorry, the page you were looking for does not exist.</p>
        </NotFoundPageWrapper>
    );
};