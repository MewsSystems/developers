import React from "react";
import Select from '@material-ui/core/Select';
import MenuItem from "@material-ui/core/MenuItem";
import FormControl from "@material-ui/core/FormControl";
import {makeStyles} from '@material-ui/core/styles';

const useStyles = makeStyles(theme => ({
    formControl: {
        margin: theme.spacing(1),
        minWidth: 220,
    },
    selectEmpty: {
        marginTop: theme.spacing(2),
    },
}));

const Filter = (props) => {
    const {config, filter, onFilterChange} = props;
    const classes = useStyles();

    const handleChange = event => {
        onFilterChange(event.target.value);
    };

    return (
        <FormControl className={classes.formControl}>
            <Select value={filter} onChange={handleChange} displayEmpty={true}>
                <MenuItem value=''>
                    {filter === '' ? 'Filter currency pairs' : 'Remove filter'}
                </MenuItem>
                {Object.keys(config).map(key => (
                    <MenuItem key={key} value={key}>
                        {config[key][0].code} / {config[key][1].code}
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    );
};

export default Filter;