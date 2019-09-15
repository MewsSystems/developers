import React, { PureComponent } from 'react';
import { LoaderWord } from './Word.styled';

interface WordProps {
    letters: number,
    spaceAfter?: boolean
}

export class Word extends PureComponent<WordProps, any>{
    render() {
        const letters = this.props.letters || 1;
        const spaceAfter = !!this.props.spaceAfter;
        return <LoaderWord className={'LoaderWord LoaderBackground' + (spaceAfter ? ' LoaderWord--WithSpaceAfter':'')} style={{width: letters + 'em'}}></LoaderWord>
    }
}