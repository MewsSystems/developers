import styled, { keyframes } from "styled-components";

const jump = keyframes`
    0%, 100% {
        transform: translateY(0);
    }

    50% {
        transform: translateY(-10px);
    }
`;

const Loader = styled.div`
    width: 100%;

    .loading {
        opacity: 0;
        display: flex;
        position: fixed;
        bottom: 50px;
        left: 50%;
        transform: translateX(-50%);
        transition: opacity 0.3s ease-in;
        position: absolute;
        top: 50%;
    }

    .loading.show {
        opacity: 1;
    }

    .ball {
        background-color: var(--btn-primary);
        border-radius: 50%;
        margin: 0 5px;
        height: 8px;
        width: 8px;
        animation: ${jump} 0.5s ease-in infinite;
    }

    .ball:nth-of-type(2) {
        animation-delay: 0.1s;
    }

    .ball:nth-of-type(3) {
        animation-delay: 0.2s;
    }
`;

export const Loading = () => {
    return (
        <Loader>
            <div className="loading show">
                <p className="mt-4 text-sm text-gray-700 absolute"> Loading movies </p>
                <div className="ball"></div>
                <div className="ball"></div>
                <div className="ball"></div>
            </div>
        </Loader>
    );
};
