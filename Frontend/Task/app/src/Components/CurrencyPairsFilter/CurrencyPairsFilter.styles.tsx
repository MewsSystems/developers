import styled from 'styled-components';

export const Wrapper = styled.div`
display: flex;
flex-direction: column;
width: 400px;
height: 100%;
border-radius: 38px;
box-shadow: 0 1px 3px rgba(0,0,0,0.12), 0 1px 2px rgba(0,0,0,0.24);
transition: all 0.3s cubic-bezier(.25,.8,.25,1);
p{
  font-weight: bold;
  border-bottom: 1px solid #ccc;
  text-align: center;
  margin: 0;
  padding: 16px 0;
}
.Filters{
  padding: 20px;
  display: flex;
  flex-wrap: wrap;
  label{
    padding: 10px;
  }
}
`;