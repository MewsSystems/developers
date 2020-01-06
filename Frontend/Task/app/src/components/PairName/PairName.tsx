import React from 'react'
import styled from 'styled-components'
import * as skin from './PairName.skin'

const PairCode = styled.span`${skin.PairCode}`;

type PairNameProps = {
    children: Array<{
        code: string;
        name: string;
    }>
    className: string
}

const PairName: React.FC<PairNameProps> = (props) => {
    const { children, className } = props;

    return (
        <span className={className}>
            {children.map(item =>
                <PairCode key={item.code}>
                    {item.code}
                </PairCode>
            )}
        </span>
    );
}

export default PairName;