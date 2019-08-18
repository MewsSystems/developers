import React from 'react';

const FilterCheckbox = (props) => {
    const isChecked = !(props.filter.filter(id => id === props.id).length > 0);
    return (
        <label>
            <input type="checkbox" name="checkbox" value={props.id} checked={isChecked} onChange={() => props.toggleFilter(props.id,isChecked)}/>
            {props.currencyPair[0].code}/{props.currencyPair[1].code}
        </label>
    );  
}
export default FilterCheckbox;

