import React, { PureComponent } from 'react';
import { Word } from './Word';

interface ParagraphProps {
    words: number
}

export class Paragraph extends PureComponent<ParagraphProps, any>{
    private randomLettersAmount(min: number = 5, max: number = 13) {
        if (max < min) {
            max = min;
        }
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }
    render() {
        const words = this.props.words || 1;
        return ' '.repeat(words).split('').map((item, index) => (
            <Word spaceAfter={index !== words - 1} letters={this.randomLettersAmount()} key={'w' + index}/>
        ));
    }
}