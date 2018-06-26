import React from 'react';
import { ListItem, FontIcon } from "mews-ui";

export default (props) => {
    const fontIconProps = {
        onClick: () => props.onClick(props.id),
        value: '▶',
    };

    return (
        <ListItem 
            primaryText={props.name}
            secondaryText={props.cost} 
            rightIcon={<FontIcon {...fontIconProps} />} 
        />
    );
};

