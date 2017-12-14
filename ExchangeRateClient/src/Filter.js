import React from 'react';

import './Filter.css';

export default function Filter({value, onChange}) {
    return <input className="Filter" value={value} onChange={onChange} />;
}
