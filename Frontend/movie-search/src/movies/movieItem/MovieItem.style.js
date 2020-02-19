import styled from 'styled-components';

const CardWrapper = styled.div`
  display: flex;
    justify-content: center;
    flex-direction: column;
    align-items: center;
    border-radius: 10px;
    background: aliceblue;
    width: 600px;
    cursor: pointer;
    margin-bottom: 15px;
`;

const CardInfoWrapper = styled.div`
  margin: 20px auto 40px;
`;


export { CardWrapper, CardInfoWrapper };
