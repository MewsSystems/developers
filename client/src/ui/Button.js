import styled from 'styled-components';

const Button = styled ('button')`
	display: flex;
	flex-direction: row;
	justify-content: center;
	align-items: center;

	min-height: 40px;
	padding: 0 18px 0 18px;

	border-radius: 4px;
	border: 1px solid black;

	background: ${p => (p.filled ? 'black' : 'white')};
	color: ${p => (p.filled ? 'white' : 'black')};

	&:not(:last-child) {
	margin: 0 10px 10px 0;
	}

	cursor: pointer;
	transition: all 0.4s ease-in-out;
`;

export default Button;