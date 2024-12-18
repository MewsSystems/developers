import React from "react";
import styled from "styled-components";
import getPosterPath from "../../utils/ui";

const PosterStyled = styled.img`
	width: 100%;
	max-width: 300px;
	border-radius: ${({ theme }) => theme.borderRadius.regular};
	object-fit: cover;
`;

const Poster = ({
	width = 300,
	posterPath,
	alt,
}: {
	width?: number;
	posterPath: string | null;
	alt: string;
}) => (
	<PosterStyled
		src={getPosterPath(posterPath, width)}
		alt={alt}
		loading="lazy"
	/>
);

export default React.memo(Poster);
