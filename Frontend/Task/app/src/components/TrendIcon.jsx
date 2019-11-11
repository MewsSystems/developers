import React from 'react'

import ExpandMoreIcon from '@material-ui/icons/ExpandMore';
import ExpandLessIcon from '@material-ui/icons/ExpandLess';
import DragHandleIcon from '@material-ui/icons/DragHandle';
import MoreHorizIcon from '@material-ui/icons/MoreHoriz';

import {makeStyles} from "@material-ui/core";

const useStyles = makeStyles(theme => ({
    growing: {
        color: 'green'
    },
    declining: {
        color: 'red'
    },
    stagnating: {
        color: 'yellow'
    }
}));

const TrendIcon = (props) => {
    const {actualRate, lastRate} = props;

    if (actualRate === null && lastRate === null) {
        return <MoreHorizIcon/>;
    }

    const trend = lastRate - actualRate;
    const classes = useStyles();

    if (trend === 0) {
        return <DragHandleIcon className={classes.stagnating}/>;
    } else if (trend > 0) {
        return <ExpandLessIcon className={classes.growing}/>;
    } else {
        return <ExpandMoreIcon className={classes.declining}/>;
    }
};

export default TrendIcon;