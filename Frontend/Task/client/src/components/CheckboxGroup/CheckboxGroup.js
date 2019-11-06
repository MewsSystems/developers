import React from 'react';
// credit to https://github.com/redux-form/redux-form/issues/1037

const CheckboxGroup = ({options = [], input: {name, value, onChange}}) => (
    <div>
        {options.map((option, index) => (
            <div
                key={`checkbox-group-item-${index}`}
            >
                <input
                    type='checkbox'
                    name={`${name}[${option.value}]`}
                    value={option.value}
                    checked={value.indexOf(option.value) !== -1}
                    onChange={event => {
                        const newValue = [...value];
                        if (event.target.checked) {
                            newValue.push(option.value);
                        } else {
                            newValue.splice(newValue.indexOf(option.value), 1);
                        }
                        return onChange(newValue);
                    }}
                />
                <label>{option.label}</label>
            </div>
        ))}
    </div>
);

export default CheckboxGroup;
