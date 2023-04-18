import styled from "styled-components";

type ImageProps =
	| HTMLImageElement
	| {
			rounded?: boolean;
	  };

const Image = styled.img<ImageProps>`
	display: block;
	width: 100%;
	height: auto;
	object-fit: cover;
	border-radius: ${(props) => (props.rounded ? "0.5rem 0.5rem 0 0" : "0")};
`;

export default Image;
