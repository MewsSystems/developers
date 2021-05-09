import React from 'react';
import styled from 'styled-components';

const Input = styled.input`
width:50%;
border:none;
height:2.5em;
border-radius:5em;
margin: 1.5em 0;
padding: 0 1em;
&:focus {
    outline: none;
    box-shadow: 0px 0px 2px red;
}

`;

const SearchBar = () => {
    return (
        <div>
            <Input type='text' name='search' value='' placeholder='find your movie here...' />
        </div>
    )
}

export default SearchBar
