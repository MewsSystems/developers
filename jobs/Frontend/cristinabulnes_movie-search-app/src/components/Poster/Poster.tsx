import React from "react";
import styled from "styled-components";

const PosterStyled = styled.img`
	width: 100%;
	max-width: 300px;
	border-radius: ${({ theme }) => theme.borderRadius.regular};
	object-fit: cover;
`;

const Poster = ({
	posterPath,
	alt,
}: {
	posterPath: string | null;
	alt: string;
}) => (
	<PosterStyled
		src={posterPath || "https://via.placeholder.com/300x450?text=No+Image"}
		alt={alt}
		loading="lazy"
	/>
);

export default React.memo(Poster);
