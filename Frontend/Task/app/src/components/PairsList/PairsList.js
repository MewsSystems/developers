import React from 'react';

import { makeStyles } from '@material-ui/core/styles';
import Grid from '@material-ui/core/Grid';
import Paper from '@material-ui/core/Paper';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import Checkbox from '@material-ui/core/Checkbox';

const useStyles = makeStyles(theme => ({    
    paper: {
      width: 200,
      height: 700,
      overflow: 'auto',
    }
  }));

export const PairsList = props => {
    const classes = useStyles();   
    
    return (
      <Grid item>
        <Paper className={classes.paper}>
          <List dense component="div" role="list">
            {props.items.map(item => {
              const labelId = `transfer-list-item-${item.id}-label`;
    
              return (
                <ListItem key={item.id} role="listitem" button onClick={props.handleToggle(item.id)}>
                  <ListItemIcon>
                    <Checkbox
                      checked={props.checked.indexOf(item.id) !== -1}
                      tabIndex={-1}
                      disableRipple
                      inputProps={{ 'aria-labelledby': labelId }}
                    />
                  </ListItemIcon>
                  <ListItemText id={labelId} primary={`${item.pair[0].name}/${item.pair[1].name} ${item.pair[0].code}/${item.pair[1].code} ${item.idx} ${item.coef}`} />
                </ListItem>
              );
            })}
            <ListItem />
          </List>
        </Paper>
      </Grid>  
    );
}