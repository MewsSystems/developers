import React from 'react';
import Button from '@material-ui/core/Button';

export const CustomButton = props => {
    return (
        <Button
            variant={props.variant}
            size={props.size}
            className={props.classes}
            onClick={props.onclick}
            disabled={props.disabled}
            aria-label={props.ariaLabel}
          >
            {props.img}
          </Button>
    )
}  