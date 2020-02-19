import styled from 'styled-components';

const MoviesDetailsWrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const MoviesDetailsContainer = styled.div`
  display: flex;
  justify-content: space-around;
  align-items: center;
  margin-top: 75px;
  width: 90%;
  padding: 22px;
  background: whitesmoke;
  border-radius: 15px;
`;

const SpinnerWrapper = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  height: 150px;
`;

const MovieDetails = styled.div`
  display: flex;
  flex-direction: column;
  flex:2;
  margin-left: 40px;
`;

const Overview = styled.div`
  display: flex;
  flex-direction: column;
`;

const Companies = styled.div`
   display: flex;
   margin-top:10px;
`;

const Company = styled.div`
  border: 3px solid #81a1b7;
  padding: 5px;
  margin-left: 5px;
`;


export { MoviesDetailsWrapper, MoviesDetailsContainer, SpinnerWrapper,MovieDetails, Overview, Company, Companies };
