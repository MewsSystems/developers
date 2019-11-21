import React from 'react';
import styled from 'styled-components';

const Loader = () => (
  <Container>
    <Image />
  </Container>
);

const Container = styled ('div')`
	display: flex;
	flex-direction: row;
	justify-content: center;
	align-items: center;

	position: fixed;
	top: 0;
	bottom: 0;
	right: 0;
	left: 0;
`;

const Image = styled ('div')`
	width: 190px;
	height: 190px;

	filter: grayscale(100%);

	background-size: cover;
	background-position: center;
	background-image: url(/loader.gif);
`;

export default Loader;
