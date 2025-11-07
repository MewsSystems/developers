import styled from "styled-components";

const Rating = styled.div<{ $rating: number }>`
    display: flex;
    align-items: center;
    justify-content: center;
    width: 2rem;
    height: 2rem;
    color: white;
    font-size: 0.8rem;
    border-radius: 0.3rem;
    flex-shrink: 0;

    background-color: ${(props) => {
        const rating = props.$rating;

        if (rating >= 8) {
            return "var(--green-500)";
        }

        if (rating >= 6) {
            return "var(--yellow-500)";
        }

        if (rating >= 4) {
            return "var(--orange-500)";
        }

        return "var(--red-500)";
    }};
`;

type Props = {
    rating?: number;
};

const MovieRating = ({ rating }: Props) => {
    return (
        <Rating $rating={rating ?? 0}>
            {rating !== undefined ? rating.toFixed(1) : "?"}
        </Rating>
    );
};

export default MovieRating;
