import React from "react";
import Input from '@material-ui/core/Input';
import FormControl from "@material-ui/core/FormControl";
import {makeStyles} from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    formControl: {
        margin: theme.spacing(1),
        minWidth: 220,
    }
}));

const Filter = (props) => {
    const {filter, onFilterChange} = props;
    const classes = useStyles();

    const handleChange = event => {
        onFilterChange(event.target.value);
    };

    return (
        <FormControl className={classes.formControl}>
            <Input value={filter} onChange={handleChange} placeholder={'Filter currency pairs'}/>
        </FormControl>
    );
};

export default Filter;