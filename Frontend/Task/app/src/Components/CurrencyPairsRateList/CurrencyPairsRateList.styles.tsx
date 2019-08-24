import styled from 'styled-components';

export const Wrapper = styled.div`
display: flex;
flex-direction: column;
width: 400px;
border-radius: 38px;
box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
transition: all 0.3s cubic-bezier(.25,.8,.25,1);
&:hover{
  box-shadow: 0 14px 28px rgba(0,0,0,0.25), 0 10px 10px rgba(0,0,0,0.22);
}
.RateListHead{
  display: flex;
  justify-content: center
  border-bottom: 1px solid #ccc;
  p{
    max-width: 100px;
    width: 100%;
    text-align: center;
    font-weight: bold;
  }
}
`;

export const CurrencyPairsWrapper = styled.div`
display: flex;
justify-content: center
align-items: center;
p{
  position: relative;
  max-width: 100px;
  width: 100%;
  text-align: center;
}
.up::after{
  position:absolute;
  top: -5px;
  left: 0;
  right: 0;
  margin: auto;
  content: "";
  width: 0;
  height: 0;
  border-bottom: solid 12px rgb(30, 200, 55);
  border-left: solid 10px transparent;
  border-right: solid 10px transparent;
}
.down::after{
  position:absolute;
  top: -5px;
  left: 0;
  right: 0;
  margin: auto;
  content: "";
  width: 0;
  height: 0;
  border-top: solid 12px rgb(200,30,50);
  border-left: solid 10px transparent;
  border-right: solid 10px transparent;
}
.stable::after{
  position:absolute;
  top: -8px;
  left: 0;
  right: 0;
  margin: auto;
  content: "Stagnating";
}
`;