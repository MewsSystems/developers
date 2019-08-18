import styled from 'styled-components';

export const Wrapper = styled.div`
display: ${props => props.error ? "flex" : "none"};;
justify-content:center;
position: absolute;
height: 100%;
width: 100%;
background: rgba(53, 52, 52, 0.6);
top: 0;
bottom: 0;
    .modal{
        width: 400px;
        height: 200px;
        background: #ffffff;
        border-radius: 38px;
        padding: 20px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        margin-top: 150px;
    }
}
`;