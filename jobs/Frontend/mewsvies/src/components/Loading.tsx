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
    position: relative;

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
        background-color: #777;
        border-radius: 50%;
        margin: 5px;
        height: 10px;
        width: 10px;
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
        <Loader className="py-80 flex flex-col">
            <p className="mt-1 text-sm text-gray-700"> Loading movies ...</p>
            <div className="loading show mt-10">
                <div className="ball"></div>
                <div className="ball"></div>
                <div className="ball"></div>
            </div>
        </Loader>
    );
};
