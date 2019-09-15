import React, { PureComponent, HTMLAttributes } from 'react';

import { IconPath } from './types';
import { IconNode } from './Icon.styled';
import renderPaths from './render-paths';

interface IconProps extends HTMLAttributes<HTMLDivElement> {
    svgPaths: IconPath[];
}

export class Icon extends PureComponent<IconProps> {
    render() {
        const { svgPaths, className = '', ...rest } = this.props;

        return (
            <IconNode
                dangerouslySetInnerHTML={{__html: renderPaths(svgPaths)}}
                className="Icon"
                {...rest}
            />
        );
    }
};