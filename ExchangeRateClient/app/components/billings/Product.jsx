import React from 'react';
import { ListItem, FontIcon } from "mews-ui";
import styled from 'styled-components';

export default (props) => {

	const onClickHandler = () => {
		props.onClick(props.id);
	};

	return (
		<ListItem
			className='listStyleItem'
			primaryText={props.name}
			secondaryText={`$${props.cost}`}
			rightIcon={<i className='fa fa-chevron-right' aria-hidden={true} onClick={onClickHandler} />}
			style={listStyle}
			onClick={onClickHandler}
		/>
	);
};

const listStyle = {
	boxShadow: '0 2px 5px 0 rgba(0,0,0,0.15)',
	width: '48%',
	display: 'inline-block',
	backgroundColor: 'white',
	borderRadius: '4px',
	marginBottom: '13px',
};

