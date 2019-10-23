import React from 'react';

import {makeStyles} from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import Checkbox from '@material-ui/core/Checkbox';

import {findIndex} from 'lodash'

const useStyles = makeStyles(theme => ({
    paper: {
        width: '500px',
        height: 300,
        overflow: 'auto',
    }
}));

export const PairsList = props => {
    if (props.type === 'right' && props.allRates.length > 0) {

        props.items.map(elem => {
            let idx = findIndex(props.allRates, {'id': elem.id})
            elem.coef = props.allRates[idx].coef
            elem.status = props.allRates[idx].status
            return elem
        })
    }

    const classes = useStyles();

    let content = <></>

    if (props.error) {
        content = (
            <Grid item>
                <Paper className={classes.paper}>
                    <List dense component="div" role="list">
                        <p>
                            Something' wrong. Please try later!
                        </p>
                    </List>
                </Paper>
            </Grid>
        )
    } else {
        content = (
            <Grid item>
                <Paper className={classes.paper}>
                    <List dense component="div" role="list">
                        {props.items.map(item => {
                            const labelId = `transfer-list-item-${item.id}-label`;
                            return (
                                <ListItem key={item.id} role="listitem" button onClick={props.handleToggle(item.id)}
                                >
                                    <ListItemIcon>
                                        <Checkbox
                                            checked={props.checked.indexOf(item.id) !== -1}
                                            tabIndex={-1}
                                            disableRipple
                                            inputProps={{'aria-labelledby': labelId}}
                                        />
                                    </ListItemIcon>
                                    <ListItemText
                                        id={labelId}
                                        primary={`${item.pair[0].name}/${item.pair[1].name} ${item.pair[0].code}/${item.pair[1].code} ${props.type === 'right' && item.coef !== undefined ? item.coef : ''} ${props.type === 'right' && item.status !== undefined ? item.status : ''}`}
                                    />
                                </ListItem>
                            );
                        })}
                        <ListItem/>
                    </List>
                </Paper>
            </Grid>
        )
    }
    return content
}
