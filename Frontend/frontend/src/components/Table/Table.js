import styled from 'styled-components';


const Table = styled.table`
  display: table;
  border-collapse: separate;
  border-spacing: 0;
  border-color: unset;
  text-align: left;
  word-break: break-word;
  transition: all 0.3s ease;


  /**
   * Header
   */
  thead {
    background: ${(p) => p.theme.grey.t100};
    user-select: none;
  }

  th {
    padding: ${(p) => p.theme.common.paddingXS};
  }

  thead tr:first-child th {
    padding: ${(p) => p.theme.common.paddingXS};
    
  }

  thead tr:not(:first-child) th {
    padding-top: 0;
    padding-bottom: ${(p) => p.theme.common.paddingXS};
    padding-left: ${(p) => p.theme.common.paddingXS};
    padding-right: ${(p) => p.theme.common.paddingXS};
  }

  .table--th-sortable {
    cursor: pointer;
  }

  .table--th-sortable:hover {
    color: ${(p) => p.theme.grey.t600};
  }

  .table--th-sortable-content {
    display: flex;
    align-items: center;
    justify-content: left;
  }

  .table--th-sortable-icon {
    margin-left: auto;
  }


  /**
   * Body
   */
  td {
    padding: ${(p) => p.theme.common.paddingXS};
  }

  tbody tr:nth-child(even) {
    background: ${(p) => p.theme.grey.t100};
  }

  tbody tr:hover {
    background: ${(p) => p.theme.grey.t300};
  }
  
  
  /**
   * Loading
   */
  .table--loading-td {
    padding: 0;
  }

  .table--loading-content {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 6rem;
    font-weight: bold;
  }


  /**
   * Error
   */
  .table--error-td {
    padding: 0;
    background: ${(p) => p.theme.error.t50};
  }

  .table--error-content {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    height: 6rem;
    font-weight: bold;
  }
`;


export default Table;
