import styled from "styled-components";
import { Props } from "./model";

const StyledImage = styled.img`
    height: 300px;
    width: 200px;
    border-radius: 5px;
    border: 1px solid black;
    box-shadow: 0 0 10px 0 rgba(0, 0, 0, 0.5);
`;

export const Poster = (props: Props) => {
    const image = props.poster_path 
    ? `https://image.tmdb.org/t/p/original${props.poster_path}` 
    : '/poster_unavailable.jpg';
    
    return (
        <StyledImage src={image} alt={props.title} />
    );
}