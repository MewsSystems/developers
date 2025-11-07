import styled from "styled-components";

const Spinner = styled.div`
    width: 2rem;
    height: 2rem;
    border-radius: 50%;
    border: 0.25rem solid var(--gray-300);
    border-top-color: var(--gray-500);
    animation: spin 1s infinite linear;

    @keyframes spin {
        to {
            transform: rotate(360deg);
        }
    }
`;

export default Spinner;
