import styled from 'styled-components';


const StyledRateListView = styled.div`
  display: inline-block;

  table {
    min-width: 20rem;
    margin: 0 auto;
  }

  th {
    min-width: 4rem;
  }

  .rateList--table-th1 {
    width: 50%;
  }

  .rateList--table-th2 {
    width: 30%
  }

  .rateList--table-th3 {
    width: 20%;
  }


  .rateList--table-td3 {
    text-align: right;
  }
`;


export default StyledRateListView;
